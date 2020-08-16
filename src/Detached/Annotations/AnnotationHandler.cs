using Detached.Model;
using System;

namespace Detached.Annotations
{
    public abstract class AnnotationHandler<TAttribute> : IAnnotationHandler
        where TAttribute : Attribute
    {
        public void Apply(Attribute annotation, ModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            Apply((TAttribute)annotation, modelOptions, typeOptions, memberOptions);
        }

        public abstract void Apply(TAttribute annotation, ModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions);
    }
}