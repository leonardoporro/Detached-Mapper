using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Collections.Generic;

namespace Detached.Mappers
{
    public static class DiscriminatorAnnotationExtensions
    {
        public static Annotation<string> DiscriminatorName(this AnnotationCollection annotations)
        {
            return annotations.Annotation<string>("DETACHED_DISCRIMINATOR_NAME");
        }

        public static Annotation<Dictionary<object, Type>> DiscriminatorValues(this AnnotationCollection annotations)
        {
            return annotations.Annotation<Dictionary<object, Type>>("DETACHED_DISCRIMINATOR_VALUES");
        }

        public static bool IsInherited(this IType type)
        {
            return type.Annotations.DiscriminatorName().Value() != null;
        }

        public static string GetDiscriminatorName(this IType type)
        {
            return type.Annotations.DiscriminatorName().Value();
        }

        public static IType SetDiscriminatorName(this IType type, string name)
        {
            type.Annotations.DiscriminatorName().Set(name);

            return type;
        }

        public static Dictionary<object, Type> GetDiscriminatorValues(this IType type)
        {
            var annotation = type.Annotations.DiscriminatorValues();

            if (!annotation.IsDefined())
            {
                annotation.Set(new());
            }

            return annotation.Value();
        }
    }
}
