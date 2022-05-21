using Detached.Mappers.Context;
using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories.Entity
{
    public class EntityRootMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.ParentTypeMap == null
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
                        Variable("created", Constant(false), out Expression created),
                        If(IsNull(typeMap.TargetExpression),
                            Then(
                                Assign(typeMap.TargetExpression, Construct(typeMap)),
                                CreateMembers(typeMap, m => m.IsKey),
                                Variable("persisted", OnMapperAction(typeMap, MapperActionType.Load), out Expression persisted),
                                If(IsNull(persisted),
                                    Then(
                                        Assign(created, Constant(true))
                                    ),
                                    Else(
                                        Assign(typeMap.TargetExpression, persisted)
                                    )
                                )
                            )
                        ),
                        CreateMembers(typeMap, m => !m.IsKey),
                        CreateBackReference(typeMap),
                        IfThenElse(created,
                            OnMapperAction(typeMap, MapperActionType.Create),
                            OnMapperAction(typeMap, MapperActionType.Update)
                        ),
                        Result(typeMap.TargetExpression)
                    )
                );
        }
    }
}