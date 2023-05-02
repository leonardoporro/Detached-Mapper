using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class AbstractAnnotationHandler
    {
    }
}

namespace Detached.Mappers
{
    public static class AbstractAnnotationHandlerExtensions
    {
        public static ClassTypeBuilder<TType> Abstract<TType> (this ClassTypeBuilder<TType> type, bool value = true)
        {
            type.TypeOptions.IsAbstract = value;
            return type;
        }
    }
}