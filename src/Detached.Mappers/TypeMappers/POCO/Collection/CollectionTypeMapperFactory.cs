using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;
using System.Linq.Expressions;
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

            Delegate construct = Lambda(
                Parameter("context", typeof(IMapContext), out Expression context),
                typePair.TargetType.BuildNewExpression(context, null)
            ).Compile();

            IType sourceItemType = mapper.Options.GetType(typePair.SourceType.ItemClrType);
            IType targetItemType = mapper.Options.GetType(typePair.TargetType.ItemClrType);
            TypePair itemTypePair = mapper.Options.GetTypePair(sourceItemType, targetItemType, typePair.ParentMember);

            ITypeMapper itemMapper = mapper.GetTypeMapper(itemTypePair);

            return (ITypeMapper)Activator.CreateInstance(mapperType, construct, itemMapper);
        }
    }
}