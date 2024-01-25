using Detached.Mappers.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class EntityAnnotationExtensions
    {
        public static Annotation<bool> Entity(this AnnotationCollection annotations)
        {
            return annotations.Annotation<bool>("DETACHED_ENTITY");
        }

        public static void Entity(this IType type, bool value = true)
        {
            if (type.MappingSchema != MappingSchema.Complex)
            {
                throw new MapperException($"Only complext types can be marked as Entities.");
            }

            type.Annotations.Entity().Set(value);
        }

        public static ClassTypeBuilder<TType> Entity<TType>(this ClassTypeBuilder<TType> type, bool value = true)
        {
            type.Type.Entity(value);

            return type;
        }

        public static bool IsEntity(this IType type)
        {
            return type.Annotations.Entity().Value();
        }
    }
}