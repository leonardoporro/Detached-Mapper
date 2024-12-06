﻿using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.Class.Complex
{
    public class ComplexTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsComplex()
                         && typePair.TargetType.IsComplex()
                         && !typePair.TargetType.IsEntity();
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            ExpressionBuilder builder = new ExpressionBuilder(mapper);

            LambdaExpression construct = builder.BuildNewExpression(typePair.TargetType);

            LambdaExpression mapMembers = builder.BuildMapMembersExpression(typePair, (s, t) => true);

            Type mapperType = typeof(ComplexTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);

            return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { construct.Compile(), mapMembers.Compile() });
        }
    }
}