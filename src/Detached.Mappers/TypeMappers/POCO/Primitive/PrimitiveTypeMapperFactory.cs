using Detached.Mappers.TypePairs;
using Detached.Mappers.Types.Class;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Primitive
{
    public class PrimitiveTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(MapperOptions mapperOptions, TypePair typePair)
        {
            return typePair.SourceType.IsPrimitive() && typePair.TargetType.IsPrimitive();
        }

        public ITypeMapper Create(MapperOptions mapperOptions, TypePair typePair)
        {
            Type mapperType = typeof(PrimitiveTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);
            return (ITypeMapper)Activator.CreateInstance(mapperType);
        }
    }
}