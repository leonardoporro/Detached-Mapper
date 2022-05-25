using Detached.Mappers.Context;
using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories.Entity
{
    public class EntityAggregatedMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return !typeMap.IsComposition
                && typeMap.TargetTypeOptions.IsEntity
                && typeMap.SourceTypeOptions.IsComplex;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                    GetDelegateType(typeMap),
                    Parameter(typeMap.SourceExpression),
                    Parameter(typeMap.TargetExpression),
                    Parameter(typeMap.BuildContextExpression),
                    Block(
                        CreateMemberMappers(typeMap, x => x.IsKey),
                        If(IsNull(typeMap.SourceExpression),
                            Then(
                                Assign(typeMap.TargetExpression, Default(typeMap.TargetExpression.Type))
                            ),
                            Else(
                                CreateKey(typeMap),
                                If(OrElse(IsNull(typeMap.TargetExpression), NotEqual(typeMap.SourceKeyExpression, typeMap.TargetKeyExpression)),
                                    Then(
                                        Assign(typeMap.TargetExpression, Construct(typeMap)),
                                        CreateMembers(typeMap, m => m.IsKey),
                                        Assign(typeMap.TargetExpression, OnMapperAction(typeMap, MapperActionType.Attach))
                                    )
                                )
                            )
                        ),
                        Result(typeMap.TargetExpression)
                    )
                );
        }
    }
}