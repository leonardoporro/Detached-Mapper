using Detached.Mappers.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class EntityTypeMapperFactory : ITypeMapperFactory
    {
        readonly MapperOptions _options;

        public EntityTypeMapperFactory(MapperOptions options)
        {
            _options = options;
        }

        public bool CanCreate(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsComplexOrEntity()
                   && targetType.IsEntity()
                   && targetType.IsConcrete();
        }

        public ITypeMapper Create(TypeMapperKey typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            ExpressionBuilder builder = new ExpressionBuilder(_options);

            LambdaExpression construct = builder.BuildNewExpression(targetType);

            builder.BuildGetKeyExpressions(sourceType, targetType, out LambdaExpression getSourceKeyExpr, out LambdaExpression getTargetKeyExpr, out Type keyType);
 
            if (typePair.Flags.HasFlag(TypeMapperKeyFlags.Root))
            {
                Type mapperType = typeof(RootEntityTypeMapper<,,>).MakeGenericType(sourceType.ClrType, targetType.ClrType, keyType);
                LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, sourceType, targetType, (s, t) => t.IsKey());
                LambdaExpression mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, sourceType, targetType, (s, t) => !t.IsKey());

                return (ITypeMapper)Activator.CreateInstance(mapperType,
                       new object[] {
                            construct.Compile(),
                            getSourceKeyExpr.Compile(),
                            getTargetKeyExpr.Compile(),
                            mapKeyMembers.Compile(),
                            mapNoKeyMembers.Compile()
                       });
            }
            else if (typePair.Flags.HasFlag(TypeMapperKeyFlags.Owned))
            {
                Type mapperType = typeof(ComposedEntityTypeMapper<,,>).MakeGenericType(sourceType.ClrType, targetType.ClrType, keyType);
                LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, sourceType, targetType, (s, t) => t.IsKey());
                LambdaExpression mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, sourceType, targetType, (s, t) => !t.IsKey());

                return (ITypeMapper)Activator.CreateInstance(mapperType,
                       new object[] {
                            construct.Compile(),
                            getSourceKeyExpr.Compile(),
                            getTargetKeyExpr.Compile(),
                            mapKeyMembers.Compile(),
                            mapNoKeyMembers.Compile()
                       });
            }
            else
            {
                Type mapperType = typeof(AggregatedEntityTypeMapper<,,>).MakeGenericType(sourceType.ClrType, targetType.ClrType, keyType);
                LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, sourceType, targetType, (s, t) => t.IsKey());

                return (ITypeMapper)Activator.CreateInstance(mapperType,
                       new object[] {
                            construct.Compile(),
                            getSourceKeyExpr.Compile(),
                            getTargetKeyExpr.Compile(),
                            mapKeyMembers.Compile()
                       });
            }
        }
    }
}