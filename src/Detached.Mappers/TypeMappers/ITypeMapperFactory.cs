using Detached.Mappers.TypePairs;

namespace Detached.Mappers.TypeMappers
{
    public interface ITypeMapperFactory
    {
        bool CanCreate(Mapper mapper, TypePair typePair);

        ITypeMapper Create(Mapper mapper, TypePair typePair);
    }
}