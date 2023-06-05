using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types.Class;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class EntityTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsComplexOrEntity()
                   && typePair.TargetType.IsEntity()
                   && typePair.TargetType.IsConcrete();
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            ExpressionBuilder builder = new ExpressionBuilder(mapper);

            LambdaExpression construct = builder.BuildNewExpression(typePair.TargetType);

            builder.BuildGetKeyExpressions(typePair, out LambdaExpression getSourceKeyExpr, out LambdaExpression getTargetKeyExpr, out Type keyType);

            if (typePair.ParentMember == null)
            {
                Type mapperType = typeof(RootEntityTypeMapper<,,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType);
                LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => t.IsKey());
                LambdaExpression mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => !t.IsKey());

                return (ITypeMapper)Activator.CreateInstance(mapperType,
                       new object[] {
                            construct.Compile(),
                            getSourceKeyExpr.Compile(),
                            getTargetKeyExpr.Compile(),
                            mapKeyMembers.Compile(),
                            mapNoKeyMembers.Compile()
                       });
            }
            else if (typePair.ParentMember.IsComposition())
            {
                Type mapperType = typeof(ComposedEntityTypeMapper<,,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType);
                LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => t.IsKey());
                LambdaExpression mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => !t.IsKey());

                return (ITypeMapper)Activator.CreateInstance(mapperType,
                       new object[] {
                            construct.Compile(),
                            getSourceKeyExpr.Compile(),
                            getTargetKeyExpr.Compile(),
                            mapKeyMembers.Compile(),
                            mapNoKeyMembers.Compile()
                       });
            }
            else if (typePair.TargetType.IsOwned() || typePair.SourceType.IsOwned())
            {
                Type mapperType = typeof(OwnedEntityTypeMapper<,,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType);
                LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => t.IsKey());
                LambdaExpression mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => !t.IsKey());

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
                Type mapperType = typeof(AggregatedEntityTypeMapper<,,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType);
                LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => t.IsKey());

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