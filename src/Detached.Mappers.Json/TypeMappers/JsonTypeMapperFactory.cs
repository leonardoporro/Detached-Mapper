using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using System;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeMappers
{
    public class JsonTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return typeof(JsonValue).IsAssignableFrom(sourceType.ClrType)
                && targetType.IsPrimitive();
        }

        public ITypeMapper Create(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            Type mapperType = typeof(JsonValueTypeMapper<>).MakeGenericType(targetType.ClrType);
            return (ITypeMapper)Activator.CreateInstance(mapperType);
        }
    }
}
