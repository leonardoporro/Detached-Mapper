using Detached.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class EntityAnnotationHandler : AnnotationHandler<EntityAttribute>
    {
        public override void Apply(EntityAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            type.Entity(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class EntityAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_ENTITY";

        public static bool IsEntity(this IType type)
        {
            return type.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static void Entity(this IType type, bool value = true)
        {
            if (type.MappingSchema != MappingSchema.Complex)
            {
                throw new MapperException($"Only complext types can be marked as Entities.");
            }

            type.Annotations[VALUE_KEY] = value;
        }

        public static ClassTypeBuilder<TType> Entity<TType>(this ClassTypeBuilder<TType> type, bool value = true)
        {
            type.Type.Entity(value);

            return type;
        }

        public static bool IsEntityConfigured(this IType type)
        {
            return type.Annotations.ContainsKey(VALUE_KEY);
        }
    }
}