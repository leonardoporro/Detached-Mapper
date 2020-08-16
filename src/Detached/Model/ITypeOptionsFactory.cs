using System;

namespace Detached.Model
{
    public interface ITypeOptionsFactory
    {
        ITypeOptions Create(ModelOptions options, Type type);
    }
}