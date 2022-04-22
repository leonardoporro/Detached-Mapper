using Detached.Mappers.TypeOptions.Types.Class;
using System;

namespace Detached.Mappers.Annotations
{
    public interface IAnnotationHandler
    {
        void Apply(Attribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions);
    }
}