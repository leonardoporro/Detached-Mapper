using Detached.Mappers.Types;
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
        const string KEY = "DETACHED_ABSTRACT";

        public static bool IsAbstract(this IType type)
        {
            return type.Annotations.ContainsKey(KEY);
        }

        public static void Abstract(this IType type, bool value)
        {
            if (value)
                type.Annotations[KEY] = true;
            else
                type.Annotations.Remove(KEY);
        }

        public static ClassTypeBuilder<TType> Abstract<TType>(this ClassTypeBuilder<TType> type, bool value = true)
        {
            type.TypeOptions.Abstract(true);
            return type;
        }
    }
}