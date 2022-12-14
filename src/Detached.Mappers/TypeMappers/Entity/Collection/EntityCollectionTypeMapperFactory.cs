using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.Entity.Collection
{
    public class EntityCollectionTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(MapperOptions mapperOptions, TypePair typePair)
        {
            if (typePair.SourceType.IsCollection()
                  && !typePair.SourceType.IsAbstract()
                  && typePair.TargetType.IsCollection()
                  && !typePair.TargetType.IsAbstract())
            {
                IType sourceItemType = mapperOptions.GetType(typePair.TargetType.ItemClrType);
                IType targetItemType = mapperOptions.GetType(typePair.TargetType.ItemClrType);

                return targetItemType.IsEntity() && sourceItemType.IsComplexOrEntity();
            }

            return false;
        }
 
        public ITypeMapper Create(MapperOptions mapperOptions, TypePair typePair)
        {
            ExpressionBuilder builder = new ExpressionBuilder(mapperOptions);

            LambdaExpression construct = builder.BuildNewExpression(typePair.TargetType);

            IType sourceItemType = mapperOptions.GetType(typePair.SourceType.ItemClrType);
            IType targetItemType = mapperOptions.GetType(typePair.TargetType.ItemClrType);
            TypePair itemTypePair = mapperOptions.GetTypePair(sourceItemType, targetItemType, typePair.ParentMember);

            ILazyTypeMapper itemMapper = mapperOptions.GetLazyTypeMapper(itemTypePair);

            builder.BuildGetKeyExpressions(itemTypePair, out LambdaExpression getSourceKeyExpr, out LambdaExpression getTargetKeyExpr, out Type keyType);

            Type baseMapperType =
                    mapperOptions.MergeCollections
                        ? typeof(MergeEntityCollectionTypeMapper<,,,,>)
                        : typeof(EntityCollectionTypeMapper<,,,,>);

            Type mapperType = baseMapperType.MakeGenericType(typePair.SourceType.ClrType, typePair.SourceType.ItemClrType, typePair.TargetType.ClrType, typePair.TargetType.ItemClrType, keyType);

            return (ITypeMapper)Activator.CreateInstance(mapperType,
                        new object[] {
                            construct.Compile(),
                            getSourceKeyExpr.Compile(),
                            getTargetKeyExpr.Compile(),
                            itemMapper
                        });
        }
    }
}