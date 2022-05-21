using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class ListMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsCollection
                && typeMap.TargetOptions.IsCollection;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.SourceExpr),
                        Parameter(typeMap.TargetExpr),
                        Parameter(typeMap.BuildContextExpr),
                        Block(
                            CreateMapper(typeMap.ItemMap),
                            If(IsNull(typeMap.SourceExpr),
                                Then(
                                    Assign(typeMap.TargetExpr, Default(typeMap.TargetExpr.Type))
                                ),
                                Else(
                                    Assign(typeMap.TargetExpr, Construct(typeMap)),
                                    Variable(typeMap.ItemMap.SourceExpr),
                                    ForEach(
                                        typeMap.ItemMap.SourceExpr,
                                        In(typeMap.SourceExpr),
                                        Block(
                                            Call("Add",
                                                typeMap.TargetExpr,
                                                CallMapper(
                                                    typeMap.ItemMap,
                                                    typeMap.ItemMap.SourceExpr,
                                                    Default(typeMap.ItemMap.TargetExpr.Type)
                                                )
                                            )
                                        )
                                    )
                                )
                            ),
                            Result(typeMap.TargetExpr)
                        )
                    );
        }

        public Expression Construct(TypeMap typeMap)
        {
            return typeMap.TargetOptions.Construct(typeMap.BuildContextExpr, null);
        }
    }
}