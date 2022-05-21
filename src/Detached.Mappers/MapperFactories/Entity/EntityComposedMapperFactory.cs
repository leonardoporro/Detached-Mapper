using Detached.Mappers.Context;
using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories.Entity
{
    public class EntityComposedMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.ParentTypeMap != null
                && typeMap.IsComposition
                && typeMap.TargetTypeOptions.IsEntity
                && typeMap.SourceTypeOptions.IsComplexType;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                    GetDelegateType(typeMap),
                    Parameter(typeMap.SourceExpression),
                    Parameter(typeMap.TargetExpression),
                    Parameter(typeMap.BuildContextExpression),
                    Block(
                        CreateMemberMappers(typeMap),
                        CreateKey(typeMap),
                        If(IsNull(typeMap.SourceExpression),
                            Then(
                                If(IsNotNull(typeMap.TargetExpression),
                                    Then(
                                        OnMapperAction(typeMap, MapperActionType.Delete),
                                        Assign(typeMap.TargetExpression, Default(typeMap.TargetExpression.Type))
                                    )
                                )
                            ),
                            Else(
                                Variable("created", Constant(false), out Expression created),
                                If(IsNull(typeMap.TargetExpression),
                                    Then(
                                        Assign(typeMap.TargetExpression, Construct(typeMap)),
                                        CreateMembers(typeMap, m => m.IsKey),
                                        Assign(created, Constant(true))
                                    ),
                                    Else(
                                        CreateKey(typeMap),
                                        If(NotEqual(typeMap.TargetKeyExpression, typeMap.SourceKeyExpression),
                                            Then(
                                                OnMapperAction(typeMap, MapperActionType.Delete),
                                                Assign(typeMap.TargetExpression, Construct(typeMap)),
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
                        Result(typeMap.TargetExpression)
                    )
                );
        }
    }
}