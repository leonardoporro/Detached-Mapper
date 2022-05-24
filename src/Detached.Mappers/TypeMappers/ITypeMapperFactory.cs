using Detached.Mappers.TypeOptions;

namespace Detached.Mappers.TypeMappers
{
    public interface ITypeMapperFactory
    {
        bool CanCreate(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType);

        ITypeMapper Create(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType);
    }
}