using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types.Class;
using System;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeMappers
{
    public class JsonTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(MapperOptions mapperOptions, TypePair typePair)
        {
            return typeof(JsonValue).IsAssignableFrom(typePair.SourceType.ClrType)
                && typePair.TargetType.IsPrimitive();
        }

        public ITypeMapper Create(MapperOptions mapperOptions, TypePair typePair)
        {
            Type mapperType = typeof(JsonValueTypeMapper<>).MakeGenericType(typePair.TargetType.ClrType);
            return (ITypeMapper)Activator.CreateInstance(mapperType);
        }
    }
}