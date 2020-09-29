using System;

namespace Detached.Mappers.Model
{
    public interface ITypeOptionsFactory
    {
        ITypeOptions Create(MapperModelOptions options, Type type);
    }
}