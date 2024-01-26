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

        public static IType SetDiscriminator(this IType type, string propertyName, Dictionary<object, Type> values)
        {
            var propertyNameAnnotation = type.Annotations.DiscriminatorName();
            var valuesAnnotation = type.Annotations.DiscriminatorValues();

            propertyNameAnnotation.Set(propertyName);
            valuesAnnotation.Set(values);

            return type;
        }

        public static bool GetDiscriminator(this IType type, out string propertyName, out Dictionary<object, Type> values)
        {
            var propertyNameAnnotation = type.Annotations.DiscriminatorName();
            var valuesAnnotation = type.Annotations.DiscriminatorValues();

            propertyName = propertyNameAnnotation.Value();
            values = valuesAnnotation.Value();

            return propertyNameAnnotation.IsDefined() && valuesAnnotation.IsDefined();
        }
    }
}
