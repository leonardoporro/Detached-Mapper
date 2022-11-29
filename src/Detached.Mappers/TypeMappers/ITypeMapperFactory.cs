using Detached.Mappers.TypeOptions;

namespace Detached.Mappers.TypeMappers
{
    public interface ITypeMapperFactory
    {
        bool CanCreate(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType);

        ITypeMapper Create(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType);
    }
}