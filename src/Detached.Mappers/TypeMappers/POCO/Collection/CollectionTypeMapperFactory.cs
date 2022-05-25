using Detached.Mappers.TypeOptions;
using System;

namespace Detached.Mappers.TypeMappers.CollectionType
{
    public class CollectionTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsCollection
                && targetType.IsCollection
                && !mapper.GetTypeOptions(targetType.ItemType).IsEntity; // TODO: simplify.
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            Type mapperType = typeof(ListTypeMapper<,>).MakeGenericType(sourceType.ItemType, targetType.ItemType);

            ILazyTypeMapper itemMapper = mapper.GetLazyTypeMapper(new TypePair(sourceType.ItemType, targetType.ItemType, typePair.Flags));

            return (ITypeMapper)Activator.CreateInstance(mapperType, itemMapper);
        }
    }
}