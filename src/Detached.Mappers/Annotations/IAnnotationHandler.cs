using Detached.Mappers.Types.Class;
using System;

namespace Detached.Mappers.Annotations
{
    public interface IAnnotationHandler
    {
        void Apply(Attribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions);
    }
}