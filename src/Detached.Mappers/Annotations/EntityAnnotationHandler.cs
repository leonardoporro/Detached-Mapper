using Detached.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class EntityAnnotationHandler : AnnotationHandler<EntityAttribute>
    {
        public override void Apply(EntityAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            typeOptions.IsEntity(true);
        }
    }

    public static class EntityAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_ENTITY";

        public static bool IsEntity(this ITypeOptions type)
        {
            return type.Annotations.ContainsKey(KEY);
        }

        public static void IsEntity(this ITypeOptions type, bool value)
        {
            if (type.Kind != TypeKind.Complex)
            {
                throw new MapperException($"Only complext types can be marked as Entities.");
            }

            if (value)
                type.Annotations[KEY] = true;
            else
                type.Annotations.Remove(KEY);
        }

        public static void IsEntity<TType>(this ClassTypeOptionsBuilder<TType> type, bool value)
        {
            type.TypeOptions.IsEntity(value);
        }
    }
}