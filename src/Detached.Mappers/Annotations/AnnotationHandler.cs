using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.Annotations
{
    public abstract class AnnotationHandler<TAttribute> : IAnnotationHandler
        where TAttribute : Attribute
    {
        public void Apply(Attribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            Apply((TAttribute)annotation, mapperOptions, type, member);
        }

        public abstract void Apply(TAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member);
    }
}