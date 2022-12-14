using Detached.Mappers.Types.Class;
using System;

namespace Detached.Mappers.Annotations
{
    public abstract class AnnotationHandler<TAttribute> : IAnnotationHandler
        where TAttribute : Attribute
    {
        public void Apply(Attribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            Apply((TAttribute)annotation, modelOptions, typeOptions, memberOptions);
        }

        public abstract void Apply(TAttribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions);
    }
}