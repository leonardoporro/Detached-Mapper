using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Primitive
{
    public class PrimitiveTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsPrimitive() && targetType.IsPrimitive();
        }

        public ITypeMapper Create(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            Type mapperType = typeof(PrimitiveTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return (ITypeMapper)Activator.CreateInstance(mapperType);
        }
    }
}