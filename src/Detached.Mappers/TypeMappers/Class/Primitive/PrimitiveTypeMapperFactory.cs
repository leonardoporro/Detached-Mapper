using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.TypeMappers.Class.Primitive
{
    public class PrimitiveTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsPrimitive() && typePair.TargetType.IsPrimitive();
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            Type mapperType = typeof(PrimitiveTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);
            return (ITypeMapper)Activator.CreateInstance(mapperType);
        }
    }
}