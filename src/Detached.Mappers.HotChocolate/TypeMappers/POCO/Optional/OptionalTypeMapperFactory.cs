using System;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types.Class;
using HotChocolate;

namespace Detached.Mappers.HotChocolate.TypeMappers.POCO.Optional
{
    public class OptionalTypeMapperFactory : ITypeMapperFactory 
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
            => typePair.SourceType.IsComplex() &&
               typeof(IOptional).IsAssignableFrom(typePair.SourceType.ClrType) &&
               typePair.TargetType.IsPrimitive();

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
            => (ITypeMapper)Activator.CreateInstance(typeof(OptionalTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType));
    }
}