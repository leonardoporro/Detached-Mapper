using AgileObjects.ReadableExpressions;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers
{
    public class EntityOwnedMapperFactory : ComplexMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsComplex
                && typeMap.TargetOptions.IsEntity
                && typeMap.Owned;
        }

        public override Delegate Create(TypeMap typeMap)
        {
            MapKey(typeMap, typeMap.Source, typeMap.Target, typeMap.Context, out Expression sourceKey, out Expression targetKey);

            LambdaExpression mapExpr =
                Expression.Lambda(
                    Block(
                        If(IsNull(typeMap.Source),
                            Then(
                                If(IsNotNull(typeMap.Target),
                                    Call("Delete", typeMap.Context, typeMap.Target)
                                ),
                                Assign(typeMap.Target, Default(typeMap.Target.Type))
                            ),
                            Else(
                                If(OrElse(IsNull(typeMap.Target), NotEqual(sourceKey, targetKey)),
                                    Then(
                                        Assign(typeMap.Target, Call("Create", typeMap.Context, new[] { typeMap.Target.Type })),
                                        If(IsNull(typeMap.Target),
                                            Assign(typeMap.Target, New(typeMap.Target.Type))
                                        )
                                    ),
                                    CreateMembers(typeMap)
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