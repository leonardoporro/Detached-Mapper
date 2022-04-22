using System;

namespace Detached.Mappers.TypeOptions
{
    public interface ITypeOptionsFactory
    {
        ITypeOptions Create(MapperOptions options, Type type);
    }
}