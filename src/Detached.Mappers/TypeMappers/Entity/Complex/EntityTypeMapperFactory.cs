﻿using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
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

            Type keyType;
            Type mapperType;
            LambdaExpression construct = builder.BuildNewExpression(typePair.TargetType);
            LambdaExpression getSourceKeyExpr;
            LambdaExpression getTargetKeyExpr;
            LambdaExpression mapKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => t.IsKey());
            LambdaExpression mapNoKeyMembers;

            builder.BuildGetKeyExpressions(typePair, out getSourceKeyExpr, out getTargetKeyExpr, out keyType);

            if (typePair.ParentMember == null)
            {
                mapperType = typeof(RootEntityTypeMapper<,,>)
                    .MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType);
                 
                mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => !t.IsKey());
            }
            else if (typePair.ParentMember.IsComposition())
            {
                mapperType = typeof(ComposedEntityTypeMapper<,,>)
                    .MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType);
                 
                mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => !t.IsKey());
            }
            else
            {
                mapperType = typeof(AggregatedEntityTypeMapper<,,>)
                    .MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, keyType); 

                mapNoKeyMembers = builder.BuildMapMembersExpression(typePair, (s, t) => mapper.Options.GetType(t.ClrType).IsPrimitive());
            }

            return (ITypeMapper)Activator.CreateInstance(
                mapperType,
                construct.Compile(),
                getSourceKeyExpr.Compile(),
                getTargetKeyExpr.Compile(),
                mapKeyMembers.Compile(),
                mapNoKeyMembers.Compile()
            );
        }
    }
}