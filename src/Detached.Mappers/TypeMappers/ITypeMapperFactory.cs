using Detached.Mappers.TypeOptions;

namespace Detached.Mappers.TypeMappers
{
    public interface ITypeMapperFactory
    {
        bool CanCreate(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType);

        ITypeMapper Create(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType);
    }
}