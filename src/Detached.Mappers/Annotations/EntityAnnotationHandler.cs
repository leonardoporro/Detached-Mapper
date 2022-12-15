using Detached.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class EntityAnnotationHandler : AnnotationHandler<EntityAttribute>
    {
        public override void Apply(EntityAttribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            typeOptions.Entity(true);
        }
    }

    public static class EntityAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_ENTITY";

        public static bool IsEntity(this IType type)
        {
            return type.Annotations.ContainsKey(KEY);
        }

        public static void Entity(this IType type, bool value = true)
        {
            if (type.MappingStrategy != MappingStrategy.Complex)
            {
                throw new MapperException($"Only complext types can be marked as Entities.");
            }

            if (value)
                type.Annotations[KEY] = true;
            else
                type.Annotations.Remove(KEY);
        }

        public static void Entity<TType>(this ClassTypeBuilder<TType> type, bool value = true)
        {
            type.TypeOptions.Entity(value);
        }
    }
}