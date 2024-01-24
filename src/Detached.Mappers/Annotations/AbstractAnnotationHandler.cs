using Detached.Annotations;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class AbstractAnnotationHandler : AnnotationHandler<AbstractAttribute>
    {
        public override void Apply(AbstractAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            type.Abstract(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class AbstractAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_ABSTRACT";

        public static bool IsAbstract(this IType type)
        {
            return type.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static void Abstract(this IType type, bool value = true)
        {
            type.Annotations[VALUE_KEY] = value;
        } 

        public static ClassTypeBuilder<TType> Abstract<TType>(this ClassTypeBuilder<TType> typeBuilder, bool value = true)
        {
            typeBuilder.Type.Abstract(value);

            return typeBuilder;
        }
    }
}