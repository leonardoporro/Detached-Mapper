using Detached.Mappers.TypePairs;

namespace Detached.Mappers.TypeMappers
{
    public interface ITypeMapperFactory
    {
        bool CanCreate(MapperOptions mapperOptions, TypePair typePair);

        ITypeMapper Create(MapperOptions mapperOptions, TypePair typePair);
    }
}