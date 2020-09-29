using AgileObjects.ReadableExpressions;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class EntityAssociatedMapperFactory : ComplexMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsComplex
                && typeMap.TargetOptions.IsEntity
                && !typeMap.Owned;
        }

        public override Delegate Create(TypeMap typeMap)
        {
            MapKey(typeMap, typeMap.Source, typeMap.Target, typeMap.Context, out Expression sourceKey, out Expression targetKey);

            LambdaExpression mapExpr =
                Expression.Lambda(
                     Block(
                        If(IsNull(typeMap.Source),
                            Then(
                                Assign(typeMap.Target, Default(typeMap.Target.Type))
                            ),
                            Else(
                                If(OrElse(IsNull(typeMap.Target), NotEqual(sourceKey, targetKey)),
                                    Then(
                                        Assign(typeMap.Target,
                                            Call("Load", typeMap.Context, new[] { typeMap.Target.Type, targetKey.Type }, sourceKey)
                                        )
                                    )
                                )
                            )
                        ),
                        Result(typeMap.Target)
                    ),
                    typeMap.Source,
                    typeMap.Target,
                    typeMap.Context
                );

            Debug.WriteLine(mapExpr.ToReadableString());

            return mapExpr.Compile();
        }
    }
}