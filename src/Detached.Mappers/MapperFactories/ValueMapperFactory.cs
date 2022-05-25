using Detached.Mappers.TypeMaps;
using System;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories
{
    public class ValueMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceTypeOptions.IsPrimitive &&
                   typeMap.TargetTypeOptions.IsPrimitive &&
                   typeof(IConvertible).IsAssignableFrom(typeMap.SourceTypeOptions.Type) &&
                   typeMap.TargetTypeOptions.Type.IsPrimitive;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                     GetDelegateType(typeMap),
                     Parameter(typeMap.SourceExpression),
                     Parameter(typeMap.TargetExpression),
                     Parameter(typeMap.BuildContextExpression),
                     Block(
                         Condition(IsNull(typeMap.SourceExpression),
                            Default(typeMap.TargetExpression.Type),
                            Call("To" + typeMap.TargetExpression.Type.Name,
                                Convert(typeMap.SourceExpression, typeof(IConvertible)),
                                Default(typeof(IFormatProvider)))
                        )
                     )
                  );
        }
    }
}