using Detached.Mappers.Types;
using HotChocolate;
using System;

namespace Detached.Mappers.HotChocolate.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsOptional(this IType type)
        {
            return type.ClrType.IsOptional();
        }

        public static bool IsOptional(this Type clrType)
        {
            return clrType.IsGenericType && clrType.GetGenericTypeDefinition() == typeof(Optional<>);
        }
    }
}