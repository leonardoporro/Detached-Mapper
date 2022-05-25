using Detached.Mappers.TypeOptions;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Object
{
    public class BoxingTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsBoxed || targetType.IsBoxed;
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            Type mapperType = typeof(BoxingTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);

            return (ITypeMapper)Activator.CreateInstance(mapperType, new object[] { mapper, typePair.Flags });
        }
    }
}