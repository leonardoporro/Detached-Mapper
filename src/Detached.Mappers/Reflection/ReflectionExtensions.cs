using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Detached.Mappers.Reflection
{
    public static class ReflectionExtensions
    {
        const int WEIGHT_EXACT_TYPE = 0;
        const int WEIGHT_INHERITED = 100;
        const int WEIGHT_IMPLEMENTED = 1000;
        const int WEIGHT_GENERIC_TYPE = 10000;
        const int WEIGHT_GENERIC_PARAMETER = 100000;
        const int WEIGHT_ERROR = int.MaxValue;

        public static ConcurrentDictionary<MethodSignature, MethodBase> Cache = new ConcurrentDictionary<MethodSignature, MethodBase>();

        public static ConstructorInfo ResolveConstructor(this Type type, Type[] paramTypes)
        {
            MethodSignature signature = new MethodSignature(type, ".ctor", paramTypes, null);

            return (ConstructorInfo)Cache.GetOrAdd(signature, key =>
            {
                int minWeight = int.MaxValue;
                ConstructorInfo minMethodInfo = null;

                foreach (ConstructorInfo ctorInfo in type.GetConstructors())
                {
                    Dictionary<Type, Type> inferred = null;
                    ParameterInfo[] paramsInfo = ctorInfo.GetParameters();

                    if (paramsInfo.Length == paramTypes.Length)
                    {
                        int methodWeight = GetWeight(paramsInfo, paramTypes, ref inferred);

                        if (methodWeight < minWeight)
                        {
                            minWeight = methodWeight;
                            minMethodInfo = ctorInfo;
                        }
                    }
                }

                return minMethodInfo;
            });
        }

        public static PropertyInfo ResolveIndexer(this Type type, Type[] paramTypes)
        {
            int minWeight = WEIGHT_ERROR;
            PropertyInfo minIndexer = null;

            foreach (PropertyInfo propInfo in type.GetProperties())
            {
                ParameterInfo[] paramsInfo = propInfo.GetIndexParameters();
                if (paramsInfo.Length == paramTypes.Length)
                {
                    Dictionary<Type, Type> inferred = null;

                    int methodWeight = GetWeight(paramsInfo, paramTypes, ref inferred);

                    if (methodWeight < minWeight)
                    {
                        minWeight = methodWeight;
                        minIndexer = propInfo;
                    }
                }
            }

            return minIndexer;
        }

        public static PropertyInfo ResolveProperty(this Type type, string propName)
        {
            foreach (PropertyInfo propInfo in type.GetTypeInfo().DeclaredProperties)
            {
                if (propInfo.Name == propName)
                    return propInfo;
            }

            foreach (Type ifaceType in type.GetInterfaces())
            {
                PropertyInfo result = ResolveProperty(ifaceType, propName);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static MethodInfo ResolveExtensionMethod(this Type type, string methodName, Type[] argTypes, Type[] paramTypes, IEnumerable<Type> usings)
        {
            MethodInfo methodInfo = null;

            Type[] newParamTypes = new Type[paramTypes.Length + 1];
            newParamTypes[0] = type;

            for (int i = 0; i < paramTypes.Length; i++)
            {
                newParamTypes[i + 1] = paramTypes[i];
            }

            foreach (Type extensionType in usings)
            {
                methodInfo = ResolveMethod(extensionType, methodName, newParamTypes, argTypes);
                if (methodInfo != null)
                    break;
            }

            return methodInfo;
        }

        public static MethodInfo ResolveMethod(this Type type, string methodName, Type[] argTypes, params Type[] paramTypes)
        {
            MethodSignature signature = new MethodSignature(type, methodName, paramTypes, argTypes);

            return (MethodInfo)Cache.GetOrAdd(signature, entry =>
            {
                List<MethodInfo> selectedMethods = new List<MethodInfo>();

                void TryAdd(MethodInfo methodInfo)
                {
                    if (methodInfo.Name == methodName)
                    {
                        selectedMethods.Add(methodInfo);
                    }
                }

                foreach (MethodInfo methodInfo in type.GetTypeInfo().DeclaredMethods)
                {
                    TryAdd(methodInfo);
                }

                Type baseType = type.BaseType;
                while (baseType != null)
                {
                    foreach (MethodInfo methodInfo in baseType.GetTypeInfo().DeclaredMethods)
                    {
                        TryAdd(methodInfo);
                    }
                    baseType = baseType.BaseType;
                }

                foreach (Type ifaceType in type.GetInterfaces())
                {
                    foreach (MethodInfo methodInfo in ifaceType.GetTypeInfo().DeclaredMethods)
                    {
                        TryAdd(methodInfo);
                    }
                }

                return SelectMethod(selectedMethods, argTypes, paramTypes);
            });
        }

        public static MethodInfo SelectMethod(IEnumerable<MethodBase> methods, Type[] argTypes, params Type[] paramTypes)
        {
            int minWeight = WEIGHT_ERROR;
            MethodInfo minMethodInfo = null;
            Dictionary<Type, Type> minInferred = null;

            foreach (MethodInfo methodInfo in methods)
            {
                Dictionary<Type, Type> inferred = null;

                int methodWeight = GetWeight(methodInfo.GetParameters(), paramTypes, ref inferred);

                if (methodWeight < minWeight)
                {
                    minWeight = methodWeight;
                    minMethodInfo = methodInfo;
                    minInferred = inferred;
                }
            }

            if (minMethodInfo != null && minMethodInfo.IsGenericMethod)
            {
                if (argTypes == null)
                {
                    argTypes = minMethodInfo.GetGenericArguments();
                    for (int i = 0; i < argTypes.Length; i++)
                    {
                        if (minInferred == null || !minInferred.TryGetValue(argTypes[i], out argTypes[i]))
                        {
                            throw new Exception($"Can't infer generic parameters on {minMethodInfo}");
                        }
                    }
                }
                minMethodInfo = minMethodInfo.MakeGenericMethod(argTypes);
            }

            return minMethodInfo;
        }

        public static int GetWeight(ParameterInfo[] paramsInfo, Type[] paramTypes, ref Dictionary<Type, Type> inferred)
        {
            int methodWeight = 0;

            if (paramTypes.Length == paramsInfo.Length)
            {
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    int weight = GetWeight(paramsInfo[i].ParameterType, paramTypes[i], ref inferred);

                    if (weight == WEIGHT_ERROR)
                    {
                        methodWeight = WEIGHT_ERROR;
                        break;
                    }
                    else
                    {
                        methodWeight += weight;
                    }
                }
            }
            else
            {
                methodWeight = WEIGHT_ERROR;
            }

            return methodWeight;
        }

        public static int GetWeight(Type destType, Type srcType, ref Dictionary<Type, Type> inferred)
        {
            if (destType.IsGenericParameter)
            {
                if (inferred == null)
                    inferred = new Dictionary<Type, Type>();

                inferred[destType] = srcType;
                return WEIGHT_GENERIC_PARAMETER;
            }

            if (destType == srcType || (destType.IsByRef && destType == srcType.MakeByRefType()))
            {
                return WEIGHT_EXACT_TYPE;
            }

            if (srcType.BaseType != null)
            {
                if (GetWeight(destType, srcType.BaseType, ref inferred) != WEIGHT_ERROR)
                    return WEIGHT_INHERITED;
            }

            if (srcType.IsGenericType && destType.IsGenericType
                && srcType.GetGenericTypeDefinition() == destType.GetGenericTypeDefinition())
            {
                Type[] srcTypes = srcType.GetGenericArguments();
                Type[] destTypes = destType.GetGenericArguments();

                bool compatible = true;
                for (int i = 0; i < srcTypes.Length; i++)
                {
                    if (GetWeight(destTypes[i], srcTypes[i], ref inferred) == WEIGHT_ERROR)
                    {
                        compatible = false;
                        break;
                    }
                }

                if (compatible)
                {
                    return WEIGHT_GENERIC_TYPE;
                }
            }

            if (destType.IsInterface)
            {
                Type[] ifaces = srcType.GetInterfaces();
                foreach (Type iface in ifaces)
                {
                    if (GetWeight(destType, iface, ref inferred) != WEIGHT_ERROR)
                        return WEIGHT_IMPLEMENTED;
                }
            }

            return WEIGHT_ERROR;
        }

        public static bool IsList(this Type type, out Type elementType)
            => IsEnumerable(type, typeof(IList<>), out elementType);

        public static bool IsEnumerable(this Type type, out Type elementType)
            => IsEnumerable(type, typeof(IEnumerable<>), out elementType);

        static bool IsEnumerable(Type type, Type iface, out Type elementType)
        {
            if (type == typeof(string))
            {
                elementType = null;
                return false;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == iface)
            {
                elementType = type.GetGenericArguments()[0];
                return true;
            }
            else
            {
                foreach (Type ifaceType in type.GetInterfaces())
                {
                    if (IsEnumerable(ifaceType, out elementType))
                    {
                        return true;
                    }
                }

                elementType = null;
                return false;
            }
        }

        public static bool IsNullable(this Type type, out Type baseType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                baseType = type.GetGenericArguments()[0];
                return true;
            }
            else
            {
                baseType = null;
                return false;
            }
        }

        public static bool IsDictionary(this Type type, out Type keyType, out Type valueType)
        {
            foreach (Type ifaceType in type.GetInterfaces())
            {
                if (ifaceType.IsGenericType && ifaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    var arguments = ifaceType.GetGenericArguments();
                    keyType = arguments[0];
                    valueType = arguments[1];
                    return true;
                }
            }

            keyType = null;
            valueType = null;
            return false;
        }

        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }

        public static bool ImplementsGenericInterface(this Type type, Type ifaceType, out Type[] args)
        {
            foreach (Type implType in type.GetInterfaces())
            {
                if (implType.IsGenericType && implType.GetGenericTypeDefinition() == ifaceType)
                {
                    args = implType.GetGenericArguments();
                    return true;
                }
            }

            args = null;
            return false;
        }
    }
}