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
            return typeMap.Parent == null
                && typeMap.TargetOptions.IsEntity
                && typeMap.SourceOptions.IsComplexType;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                    GetDelegateType(typeMap),
                    Parameter(typeMap.Source),
                    Parameter(typeMap.Target),
                    Parameter(typeMap.Context),
                    Block(
                        CreateMemberMappers(typeMap),
                        CreateKey(typeMap),
                        Variable("created", Constant(false), out Expression created),
                        If(IsNull(typeMap.Target),
                            Then(
                                Assign(typeMap.Target, Construct(typeMap)),
                                CreateMembers(typeMap, m => m.IsKey),
                                Variable("persisted", OnMapperAction(typeMap, MapperActionType.Load), out Expression persisted),
                                If(IsNull(persisted),
                                    Then(
                                        Assign(created, Constant(true))
                                    ),
                                    Else(
                                        Assign(typeMap.Target, persisted)
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
                        Result(typeMap.Target)
                    )
                );
        }
    }
}