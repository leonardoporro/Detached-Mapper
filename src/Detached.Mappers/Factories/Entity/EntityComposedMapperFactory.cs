using Detached.Mappers.Context;
using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories.Entity
{
    public class EntityComposedMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.Parent != null
                && typeMap.IsComposition
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
                        CreateMemberMappers(typeMap),
                        CreateKey(typeMap),
                        If(IsNull(typeMap.SourceExpr),
                            Then(
                                If(IsNotNull(typeMap.TargetExpr),
                                    Then(
                                        OnMapperAction(typeMap, MapperActionType.Delete),
                                        Assign(typeMap.TargetExpr, Default(typeMap.TargetExpr.Type))
                                    )
                                )
                            ),
                            Else(
                                Variable("created", Constant(false), out Expression created),
                                If(IsNull(typeMap.TargetExpr),
                                    Then(
                                        Assign(typeMap.TargetExpr, Construct(typeMap)),
                                        CreateMembers(typeMap, m => m.IsKey),
                                        Assign(created, Constant(true))
                                    ),
                                    Else(
                                        CreateKey(typeMap),
                                        If(NotEqual(typeMap.TargetKeyExpr, typeMap.SourceKeyExpr),
                                            Then(
                                                OnMapperAction(typeMap, MapperActionType.Delete),
                                                Assign(typeMap.TargetExpr, Construct(typeMap)),
                                                CreateMembers(typeMap, m => m.IsKey),
                                                Assign(created, Constant(true))
                                            )
                                        )
                                    )
                                ),
                                CreateMembers(typeMap, m => !m.IsKey),
                                CreateBackReference(typeMap),
                                IfThenElse(created,
                                    OnMapperAction(typeMap, MapperActionType.Create),
                                    OnMapperAction(typeMap, MapperActionType.Update)
                                )
                            )
                        ),
                        Result(typeMap.TargetExpr)
                    )
                );
        }
    }
}