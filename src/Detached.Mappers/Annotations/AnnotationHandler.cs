using Detached.Mappers.Types.Class;
using System;

namespace Detached.Mappers.Annotations
{
    public abstract class AnnotationHandler<TAttribute> : IAnnotationHandler
        where TAttribute : Attribute
    {
        public void Apply(Attribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            Apply((TAttribute)annotation, mapperOptions, typeOptions, memberOptions);
        }

        public abstract void Apply(TAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions);
    }
}