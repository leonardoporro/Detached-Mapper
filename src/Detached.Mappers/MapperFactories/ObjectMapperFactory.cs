using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories
{
    public class ObjectMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceExpression.Type == typeof(object) && typeMap.TargetExpression.Type != typeof(object);
        }
        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.SourceExpression),
                        Parameter(typeMap.TargetExpression),
                        Parameter(typeMap.BuildContextExpression),
                        Block(
                            If(IsNull(typeMap.SourceExpression),
                                Then(
                                    Assign(typeMap.TargetExpression, Default(typeMap.TargetExpression.Type))
                                ),
                                Else(
                                    Assign(typeMap.TargetExpression,
                                        Convert(
                                            Call("Map",
                                                Constant(typeMap.Mapper),
                                                Convert(typeMap.SourceExpression, typeof(object)),
                                                Call("GetType", typeMap.SourceExpression),
                                                Convert(typeMap.TargetExpression, typeof(object)),
                                                Constant(typeMap.TargetExpression.Type),
                                                typeMap.BuildContextExpression),
                                            typeMap.TargetExpression.Type
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