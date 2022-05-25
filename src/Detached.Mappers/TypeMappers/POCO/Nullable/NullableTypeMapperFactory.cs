using Detached.Mappers.TypeOptions;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsNullable || targetType.IsNullable;
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            if (sourceType.IsNullable && targetType.IsNullable)
            {
                Type mapperType = typeof(NullableTypeMapper<,>).MakeGenericType(sourceType.ItemType, targetType.ItemType);
                ILazyTypeMapper valueMapper = mapper.GetLazyTypeMapper(new TypePair(sourceType.ItemType, targetType.ItemType, typePair.Flags));
                return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { valueMapper });
            }
            else if (sourceType.IsNullable)
            {
                if (sourceType.ItemType == targetType.Type)
                {
                    Type mapperType = typeof(NullableSourceTypeMapper<>).MakeGenericType(sourceType.ItemType);
                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(NullableSourceTypeMapper<,>).MakeGenericType(sourceType.ItemType, targetType.Type);
                    ILazyTypeMapper valueMapper = mapper.GetLazyTypeMapper(new TypePair(sourceType.ItemType, targetType.Type, typePair.Flags));
                    return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { valueMapper });
                }
            }
            else
            {
                if (sourceType.Type == targetType.ItemType)
                {
                    Type mapperType = typeof(NullableTargetTypeMapper<>).MakeGenericType(targetType.ItemType);
                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(NullableTargetTypeMapper<,>).MakeGenericType(sourceType.Type, targetType.ItemType);
                    ILazyTypeMapper valueMapper = mapper.GetLazyTypeMapper(new TypePair(sourceType.Type, targetType.ItemType, typePair.Flags));
                    return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { valueMapper });
                }
            }
        }
    }
}