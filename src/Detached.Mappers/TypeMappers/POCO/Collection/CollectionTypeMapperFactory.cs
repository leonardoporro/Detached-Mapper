using Detached.Mappers.TypeOptions;
using System;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMappers.POCO.Collection
{
    public class CollectionTypeMapperFactory : ITypeMapperFactory
    {
        readonly MapperOptions _options;

        public CollectionTypeMapperFactory(MapperOptions options)
        {
            _options = options;
        }

        public bool CanCreate(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsCollection
                && targetType.IsCollection
                && !_options.GetTypeOptions(targetType.ItemType).IsEntity; // TODO: simplify.
        }

        public ITypeMapper Create(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            Type mapperType = typeof(CollectionTypeMapper<,,,>)
                    .MakeGenericType(sourceType.ClrType, sourceType.ItemType, targetType.ClrType, targetType.ItemType);

            Delegate construct = Lambda(New(targetType.ClrType)).Compile();
            ILazyTypeMapper itemMapper = _options.GetLazyTypeMapper(new TypePair(sourceType.ItemType, targetType.ItemType, typePair.Flags));

            return (ITypeMapper)Activator.CreateInstance(mapperType, new object[] { construct, itemMapper });
        }
    }
}