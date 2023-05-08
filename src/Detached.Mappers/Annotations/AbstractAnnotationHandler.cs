using Detached.Annotations;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class AbstractAnnotationHandler : AnnotationHandler<AbstractAttribute>
    {
        public override void Apply(AbstractAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            typeOptions.Abstract(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class AbstractAnnotationHandlerExtensions
    {
        public const string KEY = "DETACHED_ABSTRACT";

        public static bool IsAbstract(this IType type)
        {
            return type.Annotations.ContainsKey(KEY);
        }

        public static void Abstract(this IType type, bool value = true)
        {
            if (value)
                type.Annotations[KEY] = true;
            else
                type.Annotations.Remove(KEY);
        }

        public static ClassTypeBuilder<TType> Abstract<TType>(this ClassTypeBuilder<TType> type, bool value = true)
        {
            type.TypeOptions.Abstract(value);
            return type;
        }
    }
}