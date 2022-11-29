using Detached.Mappers.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Detached.Mappers.TypeOptions.Class
{
    public static class ClassTypeExtensions
    {
        public static PropertyInfo GetPropertyInfo(this IMemberOptions memberOptions)
        {
            if (memberOptions is ClassMemberOptions clrMemberOptions)
            {
                return clrMemberOptions.PropertyInfo;
            }
            else
            {
                return null;
            }
        }

        public static bool IsPrimitive(this ITypeOptions typeOptions)
        {
            return typeOptions.MappingStrategy == MappingStrategy.Primitive;
        }

        public static bool IsCollection(this ITypeOptions typeOptions)
        {
            return typeOptions.MappingStrategy == MappingStrategy.Collection;
        }

        public static bool IsComplex(this ITypeOptions typeOptions)
        {
            return typeOptions.MappingStrategy == MappingStrategy.Complex;
        }

        public static bool IsComplexOrEntity(this ITypeOptions typeOptions)
        {
            return typeOptions.IsComplex() || typeOptions.IsEntity();
        }

        public static bool IsNullable(this ITypeOptions typeOptions)
        {
            return typeOptions.MappingStrategy == MappingStrategy.Nullable;
        }

        public static bool IsAbstract(this ITypeOptions typeOptions) => typeOptions.IsAbstract;

        public static bool IsConcrete(this ITypeOptions typeOptions)
        {
            return !(typeOptions.IsAbstract() || typeOptions.IsInherited());
        }

        public static bool IsInherited(this ITypeOptions typeOptions)
        {
            return typeOptions.Annotations.ContainsKey(DISCRIMINATOR_NAME_KEY);
        }

        const string DISCRIMINATOR_NAME_KEY = "DETACHED_DISCRIMINATOR_NAME";

        public static void SetDiscriminatorName(this ITypeOptions typeOptions, string discriminatorName)
        {
            if (string.IsNullOrEmpty(discriminatorName))
            {
                typeOptions.Annotations.Remove(DISCRIMINATOR_NAME_KEY);
            }
            else
            {
                typeOptions.Annotations[DISCRIMINATOR_NAME_KEY] = discriminatorName;
            }
        }

        public static string GetDiscriminatorName(this ITypeOptions typeOptions)
        {
            typeOptions.Annotations.TryGetValue(DISCRIMINATOR_NAME_KEY, out object result);
            return result as string;
        }

        const string DISCRIMINATOR_VALUES_KEY = "DETACHED_DISCRIMINATOR_VALUES";

        public static Dictionary<object, Type> GetDiscriminatorValues(this ITypeOptions typeOptions)
        {
            if (!typeOptions.Annotations.TryGetValue(DISCRIMINATOR_VALUES_KEY, out object result))
            {
                var values = new Dictionary<object, Type>();
                typeOptions.Annotations[DISCRIMINATOR_VALUES_KEY] = values;
                return values;
            }
            else
            {
                return result as Dictionary<object, Type>;
            }
        }
    }
}
