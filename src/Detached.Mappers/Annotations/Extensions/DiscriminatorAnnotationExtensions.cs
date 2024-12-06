using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace Detached.Mappers.Annotations.Extensions
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

        public static IType SetDiscriminatorName(this IType type, string propertyName)
        {
            var propertyNameAnnotation = type.Annotations.DiscriminatorName();

            propertyNameAnnotation.Set(propertyName);

            return type;
        }

        public static IType AddDiscriminatorValue(this IType type, object value, Type concreteType)
        {
            var annotation = type.Annotations.DiscriminatorValues();

            if (annotation.IsDefined())
            {
                annotation.Value()[value] = concreteType;
            }
            else
            {
                annotation.Set(new Dictionary<object, Type> { { value, concreteType } });
            }

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
