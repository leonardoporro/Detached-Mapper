using Detached.Mappers.HotChocolate.Extensions;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.HotChocolate.TypeMappers
{
    public class OptionalTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsOptional() || typePair.TargetType.IsOptional();
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

            if (typePair.SourceType.IsOptional() && typePair.TargetType.IsOptional())
            {
                Type mapperType = typeof(OptionalTypeMapper<,>).MakeGenericType(typePair.SourceType.ItemClrType, typePair.TargetType.ItemClrType);
                ITypeMapper valueMapper = mapper.GetTypeMapper(itemTypePair);

                return (ITypeMapper)Activator.CreateInstance(mapperType, valueMapper);
            }
            else if (typePair.SourceType.IsOptional())
            {
                if (typePair.SourceType.ItemClrType == typePair.TargetType.ClrType)
                {
                    Type mapperType = typeof(OptionalSourceTypeMapper<>).MakeGenericType(typePair.SourceType.ItemClrType);

                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(OptionalSourceTypeMapper<,>).MakeGenericType(typePair.SourceType.ItemClrType, typePair.TargetType.ClrType);
                    ITypeMapper valueMapper = mapper.GetTypeMapper(itemTypePair);

                    return (ITypeMapper)Activator.CreateInstance(mapperType, valueMapper);
                }
            }
            else
            {
                if (typePair.SourceType.ClrType == typePair.TargetType.ItemClrType)
                {
                    Type mapperType = typeof(OptionalTargetTypeMapper<>).MakeGenericType(typePair.TargetType.ItemClrType);
                    return (ITypeMapper)Activator.CreateInstance(mapperType);
                }
                else
                {
                    Type mapperType = typeof(OptionalTargetTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ItemClrType);
                    ITypeMapper valueMapper = mapper.GetTypeMapper(itemTypePair);

                    return (ITypeMapper)Activator.CreateInstance(mapperType, valueMapper);
                }
            }
        }
    }
}