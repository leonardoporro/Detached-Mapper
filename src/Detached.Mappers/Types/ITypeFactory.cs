using System;

namespace Detached.Mappers.Types
{
    public interface ITypeFactory
    {
        IType Create(MapperOptions options, Type type);
    }
}