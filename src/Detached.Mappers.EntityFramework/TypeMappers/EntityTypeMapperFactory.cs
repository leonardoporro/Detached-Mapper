using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.EntityFramework.TypeMappers
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

            Type keyType;
            Type mapperType;
            LambdaExpression construct = builder.BuildNewExpression(typePair.TargetType);
            LambdaExpression getSourceKeyExpr;
            LambdaExpression getTargetKeyExpr;
            LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => t.IsKey());
            LambdaExpression mapNoKeyMembers;

            string concurrencyTokenName = typePair.TargetType.GetConcurrencyTokenName();

            builder.BuildGetKeyExpressions(typePair, out getSourceKeyExpr, out getTargetKeyExpr, out keyType);

            if (typePair.ParentMember == null || typePair.ParentMember.Annotations.Composition().Value())
            {
                mapperType = typeof(CompositionEntityTypeMapper<,,>)
                    .MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType);
                 
                mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => !t.IsKey());
            }
            else
            {
                mapperType = typeof(AggregationEntityTypeMapper<,,>)
                    .MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType); 

                mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => mapper.Options.GetType(t.ClrType).IsPrimitive());
            }

            return (ITypeMapper)Activator.CreateInstance(
                mapperType,
                construct.Compile(),
                getSourceKeyExpr.Compile(),
                getTargetKeyExpr.Compile(),
                mapKeyMembers.Compile(),
                mapNoKeyMembers.Compile(),
                concurrencyTokenName
            );
        }
    }
}