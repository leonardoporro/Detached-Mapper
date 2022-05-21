using Detached.Mappers.TypeMaps;
using System;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class ValueMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsValue &&
                   typeMap.TargetOptions.IsValue &&
                   typeof(IConvertible).IsAssignableFrom(typeMap.SourceOptions.Type) &&
                   typeMap.TargetOptions.Type.IsPrimitive;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                     GetDelegateType(typeMap),
                     Parameter(typeMap.SourceExpr),
                     Parameter(typeMap.TargetExpr),
                     Parameter(typeMap.BuildContextExpr),
                     Block(
                         Condition(IsNull(typeMap.SourceExpr),
                            Default(typeMap.TargetExpr.Type),
                            Call("To" + typeMap.TargetExpr.Type.Name, 
                                Convert(typeMap.SourceExpr, typeof(IConvertible)), 
                                Default(typeof(IFormatProvider)))
                        )
                     )
                  );
        }
    }
}