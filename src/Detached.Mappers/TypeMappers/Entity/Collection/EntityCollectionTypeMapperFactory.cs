using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.Entity.Collection
{
    public class EntityCollectionTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            if (typePair.SourceType.IsCollection()
                  && !typePair.SourceType.IsAbstract()
                  && typePair.TargetType.IsCollection()
                  && !typePair.TargetType.IsAbstract())
            {
                IType sourceItemType = mapper.Options.GetType(typePair.TargetType.ItemClrType);
                IType targetItemType = mapper.Options.GetType(typePair.TargetType.ItemClrType);

                return targetItemType.IsEntity() && sourceItemType.IsComplexOrEntity();
            }

            return false;
        }
 
        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            ExpressionBuilder builder = new ExpressionBuilder(mapper);

            LambdaExpression construct = builder.BuildNewExpression(typePair.TargetType);

            IType sourceItemType = mapper.Options.GetType(typePair.SourceType.ItemClrType);
            IType targetItemType = mapper.Options.GetType(typePair.TargetType.ItemClrType);
            TypePair itemTypePair = mapper.Options.GetTypePair(sourceItemType, targetItemType, typePair.ParentMember);

            ILazyTypeMapper itemMapper = mapper.GetLazyTypeMapper(itemTypePair);

            builder.BuildGetKeyExpressions(itemTypePair, out LambdaExpression getSourceKeyExpr, out LambdaExpression getTargetKeyExpr, out Type keyType);

            Type baseMapperType = typeof(EntityCollectionTypeMapper<,,,,>);

            Type mapperType = baseMapperType.MakeGenericType(
                typePair.SourceType.ClrType, 
                typePair.SourceType.ItemClrType, 
                typePair.TargetType.ClrType, 
                typePair.TargetType.ItemClrType, 
                keyType);

            return (ITypeMapper)Activator.CreateInstance(mapperType,
                            construct.Compile(),
                            getSourceKeyExpr.Compile(),
                            getTargetKeyExpr.Compile(),
                            itemMapper);
        }
    }
}