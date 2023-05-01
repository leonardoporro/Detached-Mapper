using Detached.Mappers.HotChocolate.Types;
using Detached.Mappers.Types;

namespace Detached.Mappers.HotChocolate.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsOptional(this IType type)
        {
            return type.Annotations.ContainsKey(OptionalClassTypeFactory.OPTIONAL_ANNOTATION);
        }
    }
}
