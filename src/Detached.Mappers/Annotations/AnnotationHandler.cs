using Detached.Mappers.TypeOptions.Types.Class;
using System;

namespace Detached.Mappers.Annotations
{
    public abstract class AnnotationHandler<TAttribute> : IAnnotationHandler
        where TAttribute : Attribute
    {
        public void Apply(Attribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            Apply((TAttribute)annotation, modelOptions, typeOptions, memberOptions);
        }

        public abstract void Apply(TAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions);
    }
}