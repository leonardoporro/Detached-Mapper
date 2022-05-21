using Detached.Mappers.Context;
using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories.Entity
{
    public class EntityAggregatedMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return !typeMap.IsComposition
                && typeMap.TargetOptions.IsEntity
                && typeMap.SourceOptions.IsComplexType;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                    GetDelegateType(typeMap),
                    Parameter(typeMap.SourceExpr),
                    Parameter(typeMap.TargetExpr),
                    Parameter(typeMap.BuildContextExpr),
                    Block(
                        CreateMemberMappers(typeMap, x => x.IsKey),
                        If(IsNull(typeMap.SourceExpr),
                            Then(
                                Assign(typeMap.TargetExpr, Default(typeMap.TargetExpr.Type))
                            ),
                            Else(
                                CreateKey(typeMap),
                                If(OrElse(IsNull(typeMap.TargetExpr), NotEqual(typeMap.SourceKeyExpr, typeMap.TargetKeyExpr)),
                                    Then(
                                        Assign(typeMap.TargetExpr, Construct(typeMap)),
                                        CreateMembers(typeMap, m => m.IsKey),
                                        Assign(typeMap.TargetExpr, OnMapperAction(typeMap, MapperActionType.Attach))
                                    )
                                )
                            )
                        ),
                        Result(typeMap.TargetExpr)
                    )
                );
        }
    }
}