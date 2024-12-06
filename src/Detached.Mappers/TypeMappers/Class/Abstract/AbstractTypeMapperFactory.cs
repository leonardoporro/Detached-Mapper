using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Exceptions;
using Detached.Mappers.Options;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.TypeMappers.Class.Abstract
{
    public class AbstractTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsAbstract() || typePair.TargetType.IsAbstract();
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            Type mapperType = typeof(AbstractTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);

            Type concreteTargetType = typePair.TargetType.IsAbstract() && !typePair.TargetType.IsInherited() && typePair.TargetType.ClrType != typeof(object)
                ? GetConcreteType(mapper.Options, typePair.TargetType.ClrType)
                : typePair.TargetType.ClrType;

            return (ITypeMapper)Activator.CreateInstance(mapperType, mapper, typePair, concreteTargetType);
        }

        public Type GetConcreteType(MapperOptions mapperOptions, Type abstractType)
        {
            if (mapperOptions.ConcreteTypes.TryGetValue(abstractType, out Type type))
            {
                return type;
            }
            else if (abstractType.IsGenericType && mapperOptions.ConcreteTypes.TryGetValue(abstractType.GetGenericTypeDefinition(), out Type genericType))
            {
                return genericType.MakeGenericType(abstractType.GetGenericArguments());
            }
            else
            {
                throw new MapperException($"Can't find a concrete type for abstract type or interface {abstractType}");
            }
        }
    }
}