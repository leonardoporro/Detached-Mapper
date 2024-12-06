using System;
using Detached.Mappers.Options;

namespace Detached.Mappers.Types
{
    public interface ITypeFactory
    {
        IType Create(MapperOptions options, Type clrType);
    }
}