using System;

namespace Detached.Mappers.Model
{
    public interface ITypeOptionsFactory
    {
        ITypeOptions Create(MapperOptions options, Type type);
    }
}