using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeBinders.Binders
{
    public class InheritedTypeBinder : ITypeBinder
    {
        public bool CanBind(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsComplex()
                && typePair.TargetType.IsComplex()
                && typePair.SourceType.GetDiscriminator(out _, out _)
                && typePair.TargetType.GetDiscriminator(out _, out _);
        }

        public Expression Bind(Mapper mapper, TypePair typePair, Expression sourceExpr)
        {
            var options = mapper.Options;

            typePair.SourceType.GetDiscriminator(out var sourceName, out var sourceValues);
            typePair.TargetType.GetDiscriminator(out var targetName, out var targetValues);

            if (sourceName != targetName)
            {
                throw new MapperException($"Discriminator members '{typePair.SourceType}.{sourceName}' and '{typePair.TargetType}.{targetName}' doesn't match.");
            }

            var member = typePair.GetMember(sourceName);
            if (member == null)
            {
                throw new MapperException($"Discriminator member '{sourceName}' must be mapped for both {typePair.SourceType} and {typePair.TargetType}");
            }

            var propertyExpr = member.SourceMember.BuildGetExpression(sourceExpr, null);

            Expression resultExpr = Constant(null, typePair.TargetType.ClrType);

            foreach (var entry in sourceValues)
            {
                var sourceValue = entry.Key;

                if (!targetValues.TryGetValue(sourceValue, out Type targetClrType))
                {
                    throw new MapperException($"Value '{sourceValue}' for discriminator '{sourceName}' doesn't have an concrete type for '{typePair.TargetType.ClrType}'");
                }

                var targetType = options.GetType(targetClrType);
                var sourceType = options.GetType(entry.Value);
                var concreteTypePair = options.GetTypePair(sourceType, targetType, typePair.ParentMember);
                var binder = mapper.GetTypeBinder(concreteTypePair);

                var conditionExpr = Equal(propertyExpr, Constant(sourceValue, sourceValue.GetType()));

                var bindExpr = binder.Bind(mapper, concreteTypePair, Convert(sourceExpr, sourceType.ClrType));

                resultExpr = Condition(conditionExpr, Convert(bindExpr, typePair.TargetType.ClrType), resultExpr);
            }

            return resultExpr;
        }
    }
}
