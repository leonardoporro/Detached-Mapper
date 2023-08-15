using Detached.RuntimeTypes.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Detached.Mappers.Types.Class
{
    public static class ClassTypeExtensions
    {
        public static PropertyInfo GetPropertyInfo(this ITypeMember memberOptions)
        {
            if (memberOptions is ClassTypeMember clrMemberOptions)
            {
                return clrMemberOptions.PropertyInfo;
            }
            else
            {
                return null;
            }
        } 

        public static bool IsPrimitive(this IType type)
        {
            return type.MappingSchema == MappingSchema.Primitive;
        }

        public static bool IsCollection(this IType type)
        {
            return type.MappingSchema == MappingSchema.Collection;
        }

        public static bool IsComplex(this IType type)
        {
            return type.MappingSchema == MappingSchema.Complex;
        }

        public static bool IsComplexOrEntity(this IType type)
        {
            return type.IsComplex() || type.IsEntity();
        }

        public static bool IsNullable(this IType type)
        {
            return type.ClrType.IsNullable(out _);
        }

        public static bool IsConcrete(this IType type)
        {
            return !(type.IsAbstract() || type.IsInherited());
        }

        public static bool IsInherited(this IType type)
        {
            return type.Annotations.ContainsKey(DISCRIMINATOR_NAME_KEY);
        }

        const string DISCRIMINATOR_NAME_KEY = "DETACHED_DISCRIMINATOR_NAME";

        public static void SetDiscriminatorName(this IType type, string discriminatorName)
        {
            if (string.IsNullOrEmpty(discriminatorName))
            {
                type.Annotations.Remove(DISCRIMINATOR_NAME_KEY);
            }
            else
            {
                type.Annotations[DISCRIMINATOR_NAME_KEY] = discriminatorName;
            }
        }

        public static string GetDiscriminatorName(this IType type)
        {
            type.Annotations.TryGetValue(DISCRIMINATOR_NAME_KEY, out object result);
            return result as string;
        }

        const string DISCRIMINATOR_VALUES_KEY = "DETACHED_DISCRIMINATOR_VALUES";

        public static Dictionary<object, Type> GetDiscriminatorValues(this IType type)
        {
            if (!type.Annotations.TryGetValue(DISCRIMINATOR_VALUES_KEY, out object result))
            {
                var values = new Dictionary<object, Type>();
                type.Annotations[DISCRIMINATOR_VALUES_KEY] = values;
                return values;
            }
            else
            {
                return result as Dictionary<object, Type>;
            }
        }

        const string CONCURRENCY_TOKEN_NAME_KEY = "DETACHED_CONCURRENCY_TOKEN_NAME";

        public static void SetConcurrencyTokenName(this IType type, string concurrencyTokenName)
        {
            if (string.IsNullOrEmpty(concurrencyTokenName))
            {
                type.Annotations.Remove(CONCURRENCY_TOKEN_NAME_KEY);
            }
            else
            {
                type.Annotations[CONCURRENCY_TOKEN_NAME_KEY] = concurrencyTokenName;
            }
        }

        public static string GetConcurrencyTokenName(this IType type)
        {
            type.Annotations.TryGetValue(CONCURRENCY_TOKEN_NAME_KEY, out object result);
            return result as string;
        }
    }
}