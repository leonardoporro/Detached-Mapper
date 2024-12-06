﻿using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Context;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMappers.Class.Complex
{
    public class KeyToComplexTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsPrimitive()
                && typePair.TargetType.IsComplex() && !typePair.TargetType.IsAbstract()
                && typePair.TargetType.GetKeyMember() != null;
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            ExpressionBuilder builder = new ExpressionBuilder(mapper);

            var sourceParam = Parameter(typePair.SourceType.ClrType, "source");
            var targetParam = Parameter(typePair.TargetType.ClrType, "target");
            var contextParam = Parameter(typeof(IMapContext), "context");

            var construct = builder.BuildNewExpression(typePair.TargetType);

            var member = typePair.TargetType.GetKeyMember();

            var setType = typeof(Action<,,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, typeof(IMapContext));

            var set = Lambda(setType, member.BuildSetExpression(targetParam, sourceParam, contextParam), sourceParam, targetParam, contextParam);

            var mapperType = typeof(KeyToComplexTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);

            return (ITypeMapper)Activator.CreateInstance(mapperType,
                construct.Compile(),
                set.Compile());
        }
    }
}
