using Detached.Mappers.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.Entity.Collection
{
    public class EntityCollectionTypeMapperFactory : ITypeMapperFactory
    {
        readonly MapperOptions _options;

        public EntityCollectionTypeMapperFactory(MapperOptions options)
        {
            _options = options;
        }

        public bool CanCreate(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            if (sourceType.IsCollection()
                   && !sourceType.IsAbstract()
                   && targetType.IsCollection()
                   && !targetType.IsAbstract())
            {
                ITypeOptions sourceItemType = _options.GetTypeOptions(targetType.ItemClrType);
                ITypeOptions targetItemType = _options.GetTypeOptions(targetType.ItemClrType);

                return targetItemType.IsEntity() && sourceItemType.IsComplexOrEntity();
            }

            return false;
        }

        public ITypeMapper Create(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            ExpressionBuilder builder = new ExpressionBuilder(_options);

            LambdaExpression construct = builder.BuildNewExpression(targetType);

            ILazyTypeMapper itemMapper = _options.GetLazyTypeMapper(new TypeMapperKey(sourceType.ItemClrType, targetType.ItemClrType, typePair.Flags));

            ITypeOptions sourceItemType = _options.GetTypeOptions(sourceType.ItemClrType);
            ITypeOptions targetItemType = _options.GetTypeOptions(targetType.ItemClrType);

            builder.BuildGetKeyExpressions(sourceItemType, targetItemType, out LambdaExpression getSourceKeyExpr, out LambdaExpression getTargetKeyExpr, out Type keyType);

            Type baseMapperType = 
                _options.MergeCollections
                    ? typeof(MergeEntityCollectionTypeMapper<,,,,>)
                    : typeof(EntityCollectionTypeMapper<,,,,>);

            Type mapperType = baseMapperType.MakeGenericType(sourceType.ClrType, sourceType.ItemClrType, targetType.ClrType, targetType.ItemClrType, keyType);

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