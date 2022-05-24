using Detached.Mappers.Context;
using Detached.Mappers.TypeOptions;
using System;

namespace Detached.Mappers.TypeMappers.PrimitiveType
{
    public class PrimitiveTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsPrimitiveType && targetType.IsPrimitiveType;
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            Type mapperType = typeof(PrimitiveTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return (ITypeMapper)Activator.CreateInstance(mapperType);
        }
    }
}