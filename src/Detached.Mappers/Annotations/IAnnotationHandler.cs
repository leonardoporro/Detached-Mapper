using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.Annotations
{
    public interface IAnnotationHandler
    {
        void Apply(Attribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member);
    }
}