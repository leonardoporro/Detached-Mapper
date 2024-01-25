using Detached.Mappers.Annotations;
using Detached.Mappers.Types;

namespace Detached.Mappers
{
    public static class ConcurrencyTokenAnnotationExtensions
    {
        public static Annotation<string> ConcurrencyTokenName(this AnnotationCollection annotations)
        {
            return annotations.Annotation<string>("DETACHED_CONCURRENCY_TOKEN_NAME");
        }

        public static void SetConcurrencyTokenName(this IType type, string name)
        {
            type.Annotations.ConcurrencyTokenName().Set(name);
        }

        public static string GetConcurrencyTokenName(this IType type)
        {
            return type.Annotations.ConcurrencyTokenName().Value();
        }
    }
}
