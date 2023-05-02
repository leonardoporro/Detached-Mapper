using Detached.Mappers.Annotations;
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

        public static bool IsPrimitive(this IType typeOptions)
        {
            return typeOptions.MappingSchema == MappingSchema.Primitive;
        }

        public static bool IsCollection(this IType typeOptions)
        {
            return typeOptions.MappingSchema == MappingSchema.Collection;
        }

        public static bool IsComplex(this IType typeOptions)
        {
            return typeOptions.MappingSchema == MappingSchema.Complex;
        }

        public static bool IsComplexOrEntity(this IType typeOptions)
        {
            return typeOptions.IsComplex() || typeOptions.IsEntity();
        }

        public static bool IsNullable(this IType typeOptions)
        {
            return typeOptions.ClrType.IsNullable(out _);
        }

        public static bool IsConcrete(this IType typeOptions)
        {
            return !(typeOptions.IsAbstract() || typeOptions.IsInherited());
        }

        public static bool IsInherited(this IType typeOptions)
        {
            return typeOptions.Annotations.ContainsKey(DISCRIMINATOR_NAME_KEY);
        }

        const string DISCRIMINATOR_NAME_KEY = "DETACHED_DISCRIMINATOR_NAME";

        public static void SetDiscriminatorName(this IType typeOptions, string discriminatorName)
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

        public static string GetDiscriminatorName(this IType typeOptions)
        {
            typeOptions.Annotations.TryGetValue(DISCRIMINATOR_NAME_KEY, out object result);
            return result as string;
        }

        const string DISCRIMINATOR_VALUES_KEY = "DETACHED_DISCRIMINATOR_VALUES";

        public static Dictionary<object, Type> GetDiscriminatorValues(this IType typeOptions)
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
