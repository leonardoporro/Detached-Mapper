using Detached.Mappers.Reflection.Compiler;
using FastExpressionCompiler.Detached.Mappers.Reflection.Compiler.Detached.Mappers.Reflection.Compiler.Detached.Mappers.RuntimeTypes.Compiler;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Detached.Mappers.Reflection
{
    public class RuntimeTypeBuilder
    {
        // hacks!!!
        static FieldInfo _methodsField = typeof(TypeBuilder).GetField("m_listMethods", BindingFlags.NonPublic | BindingFlags.Instance);

        public static AssemblyBuilder DefaultAssemblyBuilder { get; } =
            AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("Detached.Mappers.RuntimeTypes"),
                AssemblyBuilderAccess.RunAndCollect
            );

        public static ModuleBuilder DefaultModuleBuilder { get; }
            = DefaultAssemblyBuilder.DefineDynamicModule("RuntimeTypesModule");

        public RuntimeTypeBuilder(
            string typeName,
            Type baseType = null,
            TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public,
            ModuleBuilder moduleBuilder = null)
        {
            if (baseType == null)
                baseType = typeof(object);

            TypeBuilder = (moduleBuilder ?? DefaultModuleBuilder).DefineType(typeName, typeAttributes, baseType);
            This = Expression.Parameter(TypeBuilder, "this");
            Base = new BaseClassExpression(This);
        }

        public TypeBuilder TypeBuilder { get; }

        public ParameterExpression This { get; }

        public BaseClassExpression Base { get; }

        public Dictionary<string, FieldBuilder> Fields { get; } = new Dictionary<string, FieldBuilder>();

        public Dictionary<string, PropertyBuilder> Properties { get; } = new Dictionary<string, PropertyBuilder>();

        public List<RuntimeTypeMethod> Methods { get; } = new List<RuntimeTypeMethod>();

        public FieldBuilder DefineField(string fieldName, Type fieldType, FieldAttributes attributes = FieldAttributes.Public)
        {
            FieldBuilder newField = TypeBuilder.DefineField(fieldName, fieldType, attributes);
            Fields.Add(fieldName, newField);
            return newField;
        }

        public PropertyBuilder DefineAutoProperty(string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = TypeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            MethodAttributes methodFlags =
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig |
                MethodAttributes.Virtual |
                MethodAttributes.CheckAccessOnOverride;

            // getter
            MethodBuilder getterMethodBuilder = TypeBuilder.DefineMethod(
                "get_" + propertyName,
                methodFlags,
                propertyType,
                Type.EmptyTypes);

            ILGenerator getIl = getterMethodBuilder.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setterMethodBuilder = TypeBuilder.DefineMethod("set_" + propertyName, methodFlags, null, new[] { propertyType });

            // setter
            ILGenerator setIl = setterMethodBuilder.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            PropertyBuilder propertyBuilder = TypeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            propertyBuilder.SetGetMethod(getterMethodBuilder);
            propertyBuilder.SetSetMethod(setterMethodBuilder);

            Fields.Add(fieldBuilder.Name, fieldBuilder);
            Properties.Add(propertyBuilder.Name, propertyBuilder);
            Methods.Add(new RuntimeTypeMethod(getterMethodBuilder.Name, getterMethodBuilder, new Type[0], propertyType));
            Methods.Add(new RuntimeTypeMethod(setterMethodBuilder.Name, setterMethodBuilder, new Type[] { propertyType }, typeof(void)));

            return propertyBuilder;
        }

        public MethodBuilder DefineMethod(
            string methodName,
            ParameterExpression[] paramExprs,
            Expression bodyExpr,
            MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual,
            CallingConventions callingConventions = CallingConventions.Standard | CallingConventions.HasThis)
        {
            if (paramExprs == null)
                paramExprs = Array.Empty<ParameterExpression>();

            Type[] paramTypes = new Type[paramExprs.Length];
            string[] paramNames = new string[paramExprs.Length];

            for (int i = 0; i < paramExprs.Length; i++)
            {
                paramTypes[i] = paramExprs[i].Type;
                paramNames[i] = paramExprs[i].Name;
            }

            if (callingConventions.HasFlag(CallingConventions.HasThis))
            {
                ParameterExpression[] newParams = new ParameterExpression[paramExprs.Length + 1];
                newParams[0] = This;
                Array.Copy(paramExprs, 0, newParams, 1, paramExprs.Length);
                paramExprs = newParams;
            }

            MethodBuilder newMethod = TypeBuilder.DefineMethod(
                methodName,
                methodAttributes,
                callingConventions,
                bodyExpr.Type,
                paramTypes);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                newMethod.DefineParameter(i, ParameterAttributes.None, paramNames[i]);
            }

            bool success = ExpressionCompiler.CompileForTypeBuilder(paramExprs, bodyExpr, bodyExpr.Type, newMethod.GetILGenerator(), true);
            if (!success)
            {
                throw new InvalidOperationException($"Can't compile method");
            }

            Methods.Add(new RuntimeTypeMethod(methodName, newMethod, paramTypes, bodyExpr.Type));

            return newMethod;
        }

        public MethodInfo OverrideMethod(string methodName, ParameterExpression[] paramExprs, Expression bodyExpr)
        {
            if (TypeBuilder.BaseType == null)
                throw new InvalidOperationException($"No base type is defined");

            Type[] paramTypes = new Type[paramExprs.Length - 1];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                paramTypes[i] = paramExprs[i + 1].Type;
            }

            MethodInfo methodInfo = TypeBuilder.BaseType.ResolveMethod(methodName, null, paramTypes);
            if (methodInfo == null)
                throw new InvalidOperationException($"No suitable method '{methodName}' found to override.");


            return OverrideMethod(methodInfo, paramExprs, bodyExpr);
        }

        public MethodInfo OverrideMethod(MethodInfo methodInfo, ParameterExpression[] paramExprs, Expression bodyExpr)
        {
            if (paramExprs == null)
                paramExprs = Array.Empty<ParameterExpression>();

            MethodAttributes methodAttributes = methodInfo.Attributes;

            if (methodAttributes.HasFlag(MethodAttributes.Abstract))
            {
                methodAttributes &= ~MethodAttributes.Abstract;
                methodAttributes |= MethodAttributes.Virtual;
            }

            if (bodyExpr.Type != methodInfo.ReturnType)
                throw new ArgumentException($"Return type mismatch: '{bodyExpr.Type}' -> '{methodInfo.ReturnType}'");

            ParameterInfo[] methodParams = methodInfo.GetParameters();
            if (methodParams.Length != paramExprs.Length)
                throw new ArgumentException($"Parameter count mismatch.");

            for (int i = 0; i < methodParams.Length; i++)
            {
                if (methodParams[i].ParameterType != paramExprs[i].Type)
                {
                    throw new ArgumentException($"Parameter {methodParams[i].Name} type mismatch: '{paramExprs[i].Type}' -> '{methodParams[i].ParameterType}'");
                }
            }

            MethodBuilder newMethod = DefineMethod(methodInfo.Name, paramExprs, bodyExpr, methodAttributes);

            TypeBuilder.DefineMethodOverride(newMethod, methodInfo);

            return newMethod;
        }

        public void AutoImplementInterface(Type type)
        {
            TypeBuilder.AddInterfaceImplementation(type);

            foreach (MethodInfo ifaceMethodInfo in type.GetTypeInfo().DeclaredMethods)
            {
                MethodInfo declaredMehtod = GetCandidateDeclaredMethod(ifaceMethodInfo);
                if (declaredMehtod != null)
                {
                    TypeBuilder.DefineMethodOverride(declaredMehtod, ifaceMethodInfo);
                }
            }

            foreach (Type ifaceType in type.GetInterfaces())
            {
                AutoImplementInterface(ifaceType);
            }
        }

        public MethodInfo GetCandidateDeclaredMethod(MethodInfo methodInfo)
        {
            MethodInfo result = null;

            foreach (RuntimeTypeMethod declaredMehtod in Methods)
            {
                if (methodInfo.Name == declaredMehtod.Name
                    && methodInfo.ReturnType == declaredMehtod.ReturnType)
                {
                    var parameters = methodInfo.GetParameters();
                    if (parameters.Length == declaredMehtod.ParameterTypes.Length)
                    {
                        result = declaredMehtod.Method;

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i].ParameterType != declaredMehtod.ParameterTypes[i])
                            {
                                result = null;
                                break;
                            }
                        }
                    }
                }

                if (result != null)
                    break;
            }

            return result;
        }
         
        public Type Create() => TypeBuilder.CreateTypeInfo();

        public static MethodInfo[] GetDeclaredMethods(TypeBuilder tb)
        {
            return ((List<MethodBuilder>)_methodsField.GetValue(tb)).ToArray();
        }
    }
}