﻿using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.PatchTypes;
using Detached.RuntimeTypes.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Types.Class
{
    public class ClassTypeFactory : ITypeFactory
    {
        public virtual IType Create(MapperOptions options, Type clrType)
        {
            ClassType classType = new ClassType();
            classType.ClrType = clrType;
            classType.Abstract(clrType == typeof(object) || clrType.IsAbstract || clrType.IsInterface);

            if (options.IsPrimitive(clrType))
            {
                classType.MappingSchema = MappingSchema.Primitive;
            }
            else if (clrType.IsEnumerable(out Type itemType))
            {
                classType.ItemClrType = itemType;
                classType.MappingSchema = MappingSchema.Collection;
            }
            else
            {
                classType.MappingSchema = MappingSchema.Complex;

                // generate members.
                foreach (PropertyInfo propInfo in clrType.GetRuntimeProperties())
                {
                    if (ShouldMap(propInfo))
                    {
                        ClassTypeMember memberOptions = CreateMember(classType, propInfo);
                        classType.Members.Add(memberOptions);
                    }
                }
            }

            if (clrType.IsNullable(out Type baseType))
            {
                classType.MappingSchema = MappingSchema.None;
                classType.ItemClrType = baseType;
            }

            CreateConstructor(classType);

            return classType;
        }

        protected virtual void CreateConstructor(ClassType classType)
        {
            ConstructorInfo constructorInfo = Array.Find(classType.ClrType.GetConstructors(), c => c.GetParameters().Length == 0);
            if (!classType.IsAbstract() && constructorInfo != null)
            {
                classType.Constructor = Lambda(New(constructorInfo));
            }
        }

        protected virtual ClassTypeMember CreateMember(ClassType classType, PropertyInfo propInfo)
        {
            ClassTypeMember memberOptions = new ClassTypeMember();
            memberOptions.Name = propInfo.Name;
            memberOptions.ClrType = propInfo.PropertyType;
            memberOptions.PropertyInfo = propInfo;

            if (typeof(IPatch).IsAssignableFrom(classType.ClrType))
            {
                memberOptions.TryGetter =
                    Lambda(
                        Parameter(classType.ClrType, out Expression instanceExpr),
                        Parameter(propInfo.PropertyType, out Expression outVar),
                        Block(
                            Variable(typeof(bool), out Expression resultExpr),
                            If(Call("IsSet", Convert(instanceExpr, typeof(IPatch)), Constant(memberOptions.Name)),
                                Block(
                                    Assign(outVar, Property(instanceExpr, propInfo)),
                                    Assign(resultExpr, Constant(true))
                                ),
                                Block(
                                    Assign(outVar, Default(outVar.Type)),
                                    Assign(resultExpr, Constant(false))
                                )
                            ),
                            resultExpr
                        )
                    );
            }

            // generate getter.
            if (propInfo.CanRead)
            {
                memberOptions.Getter = Lambda(
                        Parameter(classType.ClrType, out Expression instanceExpr),
                        Property(instanceExpr, propInfo)
                    );
            }

            // generate setter.
            if (propInfo.CanWrite)
            {
                memberOptions.Setter = Lambda(
                       Parameter(classType.ClrType, out Expression instanceExpr),
                       Parameter(propInfo.PropertyType, out Expression valueExpr),
                       Assign(Property(instanceExpr, propInfo), valueExpr)
                   );
            }

            return memberOptions;
        }

        public virtual bool ShouldMap(PropertyInfo propInfo)
        {
            bool result = true;

            if (propInfo.Name == "Modified"
                && typeof(IPatch).IsAssignableFrom(propInfo.DeclaringType))
            {
                result = false;
            }

            return result;
        }
    }
}