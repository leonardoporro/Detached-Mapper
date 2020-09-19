using System;

namespace Detached.Model
{
    public interface ITypeOptionsFactory
    {
        ITypeOptions Create(MapperModelOptions options, Type type);
    }
}