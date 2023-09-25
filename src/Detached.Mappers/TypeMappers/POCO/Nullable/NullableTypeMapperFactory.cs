using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsNullable() || typePair.TargetType.IsNullable();
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            IType sourceItemType = typePair.SourceType.ItemClrType != null
                ? mapper.Options.GetType(typePair.SourceType.ItemClrType)
                : typePair.SourceType;

            IType targetItemType = typePair.TargetType.ItemClrType != null
                ? mapper.Options.GetType(typePair.TargetType.ItemClrType)
                : typePair.TargetType;

            TypePair itemTypePair = mapper.Options.GetTypePair(sourceItemType, targetItemType, typePair.ParentMember);

            if (typePair.SourceType.IsNullable() && typePair.TargetType.IsNullable())
            {
                Type mapperType = typeof(NullableTypeMapper<,>).MakeGenericType(typePair.SourceType.ItemClrType, typePair.TargetType.ItemClrType);
                ITypeMapper valueMapper = mapper.GetTypeMapper(itemTypePair);

                return (ITypeMapper)Activator.CreateInstance(mapperType, valueMapper);
            }
            else if (typePair.SourceType.IsNullable())
            {
                if (typePair.SourceType.ItemClrType == typePair.TargetType.ClrType)
                {
                    Type mapperType = typeof(NullableSourceTypeMapper<>).MakeGenericType(typePair.SourceType.ItemClrType);

                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(NullableSourceTypeMapper<,>).MakeGenericType(typePair.SourceType.ItemClrType, typePair.TargetType.ClrType);
                    ITypeMapper valueMapper = mapper.GetTypeMapper(itemTypePair);

                    return (ITypeMapper)Activator.CreateInstance(mapperType, valueMapper);
                }
            }
            else
            {
                if (typePair.SourceType.ClrType == typePair.TargetType.ItemClrType)
                {
                    Type mapperType = typeof(NullableTargetTypeMapper<>).MakeGenericType(typePair.TargetType.ItemClrType);
                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(NullableTargetTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ItemClrType);
                    ITypeMapper valueMapper = mapper.GetTypeMapper(itemTypePair);

                    return (ITypeMapper)Activator.CreateInstance(mapperType, valueMapper);
                }
            }
        }
    }
}