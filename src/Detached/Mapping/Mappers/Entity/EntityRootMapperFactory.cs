using Detached.Mapping.Context;
using Detached.Mapping.Mappers.Entity;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers
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
                                Assign(typeMap.Target, typeMap.TargetOptions.Construct(typeMap.Context)),
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