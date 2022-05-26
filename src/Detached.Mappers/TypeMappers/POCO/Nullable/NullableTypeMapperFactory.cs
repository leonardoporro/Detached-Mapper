using Detached.Mappers.TypeOptions;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableTypeMapperFactory : ITypeMapperFactory
    {
        readonly MapperOptions _options;

        public NullableTypeMapperFactory(MapperOptions options)
        {
            _options = options;
        }

        public bool CanCreate(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsNullable || targetType.IsNullable;
        }

        public ITypeMapper Create(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            if (sourceType.IsNullable && targetType.IsNullable)
            {
                Type mapperType = typeof(NullableTypeMapper<,>).MakeGenericType(sourceType.ItemType, targetType.ItemType);
                ILazyTypeMapper valueMapper = _options.GetLazyTypeMapper(new TypePair(sourceType.ItemType, targetType.ItemType, typePair.Flags));
                return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { valueMapper });
            }
            else if (sourceType.IsNullable)
            {
                if (sourceType.ItemType == targetType.ClrType)
                {
                    Type mapperType = typeof(NullableSourceTypeMapper<>).MakeGenericType(sourceType.ItemType);
                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(NullableSourceTypeMapper<,>).MakeGenericType(sourceType.ItemType, targetType.ClrType);
                    ILazyTypeMapper valueMapper = _options.GetLazyTypeMapper(new TypePair(sourceType.ItemType, targetType.ClrType, typePair.Flags));
                    return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { valueMapper });
                }
            }
            else
            {
                if (sourceType.ClrType == targetType.ItemType)
                {
                    Type mapperType = typeof(NullableTargetTypeMapper<>).MakeGenericType(targetType.ItemType);
                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(NullableTargetTypeMapper<,>).MakeGenericType(sourceType.ClrType, targetType.ItemType);
                    ILazyTypeMapper valueMapper = _options.GetLazyTypeMapper(new TypePair(sourceType.ClrType, targetType.ItemType, typePair.Flags));
                    return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { valueMapper });
                }
            }
        }
    }
}