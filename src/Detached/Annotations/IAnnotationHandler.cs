using Detached.Model;
using System;

namespace Detached.Annotations
{
    public interface IAnnotationHandler
    {
        public abstract void Apply(Attribute annotation, MapperModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions);
    }
}