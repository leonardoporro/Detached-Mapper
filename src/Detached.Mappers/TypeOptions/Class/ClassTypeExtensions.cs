using Detached.Mappers.Annotations;
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
            return typeOptions.Kind == TypeKind.Primitive;
        }

        public static bool IsCollection(this ITypeOptions typeOptions)
        {
            return typeOptions.Kind == TypeKind.Collection;
        }

        public static bool IsComplex(this ITypeOptions typeOptions)
        {
            return typeOptions.Kind == TypeKind.Complex;
        }

        public static bool IsComplexOrEntity(this ITypeOptions typeOptions)
        {
            return typeOptions.IsComplex() || typeOptions.IsEntity();
        }

        public static bool IsNullable(this ITypeOptions typeOptions)
        {
            return typeOptions.Kind == TypeKind.Nullable;
        }

        public static bool IsAbstract(this ITypeOptions typeOptions)
        {
            return typeOptions.ClrType == typeof(object) || typeOptions.ClrType.IsAbstract || typeOptions.ClrType.IsInterface;
        }

        public static bool IsConcrete(this ITypeOptions typeOptions)
        {
            return !(typeOptions.IsAbstract() || typeOptions.IsInherited());
        }

        public static bool IsInherited(this ITypeOptions typeOptions)
        {
            return typeOptions.DiscriminatorName != null;
        }
    }
}
