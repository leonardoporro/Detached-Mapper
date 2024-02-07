using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Collections.Generic;

namespace Detached.Mappers
{
    public static class MapToAnnotationExtensions
    {
        public static Annotation<Dictionary<Type, string>> MapTo(this AnnotationCollection annotations)
        {
            return annotations.Annotation<Dictionary<Type, string>>("DETACHED_MAP_TO");
        }

        public static Annotation<Dictionary<Type, string>> MapFrom(this AnnotationCollection annotations)
        {
            return annotations.Annotation<Dictionary<Type, string>>("DETACHED_MAP_FROM");
        }

        public static void MapTo(this ITypeMember member, Type sourceType, string targetMemberName)
        {
            var mapToAnnotation = member.Annotations.MapTo();

            if (!mapToAnnotation.IsDefined())
            {
                mapToAnnotation.Set(new());
            }

            mapToAnnotation.Value()[sourceType] = targetMemberName;
        }

        public static void MapFrom(this ITypeMember member, Type targetType, string sourceMemberName)
        {
            var mapToAnnotation = member.Annotations.MapFrom();

            if (!mapToAnnotation.IsDefined())
            {
                mapToAnnotation.Set(new());
            }

            mapToAnnotation.Value()[targetType] = sourceMemberName;
        }
    }
}