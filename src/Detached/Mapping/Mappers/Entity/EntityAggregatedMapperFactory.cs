using Detached.Mapping.Context;
using Detached.Mapping.Mappers.Entity;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers
{
    public class EntityAggregatedMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return !typeMap.Owned
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
                        CreateMemberMappers(typeMap, x => x.IsKey),
                        If(IsNull(typeMap.Source),
                            Then(
                                Assign(typeMap.Target, Default(typeMap.Target.Type))
                            ),
                            Else(
                                CreateKey(typeMap),
                                If(OrElse(IsNull(typeMap.Target), NotEqual(typeMap.SourceKey, typeMap.TargetKey)),
                                    Then(
                                        Assign(typeMap.Target, typeMap.TargetOptions.Construct(typeMap.Context)),
                                        CreateMembers(typeMap, m => m.IsKey),
                                        Assign(typeMap.Target, OnMapperAction(typeMap, MapperActionType.Attach))
                                    )
                                ),
                                CreateBackReference(typeMap)
                            )
                        ),
                        Result(typeMap.Target)
                    )
                );
        }
    }
}