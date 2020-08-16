using Detached.Model;
using System;

namespace Detached.Annotations
{
    public interface IAnnotationHandler
    {
        public abstract void Apply(Attribute annotation, ModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions);
    }
}