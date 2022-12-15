using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using System;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMappers.POCO.Collection
{
    public class CollectionTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsCollection()
               && typePair.TargetType.IsCollection()
               && !mapper.Options.GetType(typePair.TargetType.ItemClrType).IsEntity(); // TODO: simplify.
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            Type mapperType = typeof(CollectionTypeMapper<,,,>)
                   .MakeGenericType(typePair.SourceType.ClrType, typePair.SourceType.ItemClrType, typePair.TargetType.ClrType, typePair.TargetType.ItemClrType);

            Delegate construct = Lambda(New(typePair.TargetType.ClrType)).Compile();

            IType sourceItemType = mapper.Options.GetType(typePair.SourceType.ItemClrType);
            IType targetItemType = mapper.Options.GetType(typePair.TargetType.ItemClrType);
            TypePair itemTypePair = mapper.Options.GetTypePair(sourceItemType, targetItemType, typePair.ParentMember);

            ILazyTypeMapper itemMapper = mapper.GetLazyTypeMapper(itemTypePair);

            return (ITypeMapper)Activator.CreateInstance(mapperType, new object[] { construct, itemMapper });
        }
    }
}