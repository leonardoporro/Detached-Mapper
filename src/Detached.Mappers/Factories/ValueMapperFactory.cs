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
                     Parameter(typeMap.Source),
                     Parameter(typeMap.Target),
                     Parameter(typeMap.Context),
                     Block(
                         Condition(IsNull(typeMap.Source),
                            Default(typeMap.Target.Type),
                            Call("To" + typeMap.Target.Type.Name, 
                                Convert(typeMap.Source, typeof(IConvertible)), 
                                Default(typeof(IFormatProvider)))
                        )
                     )
                  );
        }
    }
}