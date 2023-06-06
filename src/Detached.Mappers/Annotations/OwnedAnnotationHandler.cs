using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;
using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.Annotations
{
    public class OwnedAnnotationHandler : AnnotationHandler<OwnedAttribute>
    {
        public override void Apply(OwnedAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions,
            ClassTypeMember memberOptions)
        {
            typeOptions.Owned(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class OwnedAnnotationHandlerExtensions
    {
        public const string KEY = "DETACHED_OWNED";

        public static bool IsOwned(this IType type)
        {
            return type.Annotations.ContainsKey(KEY);
        }

        public static void Owned(this IType type, bool value = true)
        {
            if (value)
                type.Annotations[KEY] = true;
            else
                type.Annotations.Remove(KEY);
        }

        public static ClassTypeBuilder<TType> Owned<TType>(this ClassTypeBuilder<TType> type, bool value = true)
        {
            type.TypeOptions.Owned(value);
            return type;
        }
    }
}