using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class ObjectMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceExpr.Type == typeof(object) && typeMap.TargetExpr.Type != typeof(object);
        }
        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.SourceExpr),
                        Parameter(typeMap.TargetExpr),
                        Parameter(typeMap.BuildContextExpr),
                        Block(
                            If(IsNull(typeMap.SourceExpr),
                                Then(
                                    Assign(typeMap.TargetExpr, Default(typeMap.TargetExpr.Type))
                                ),
                                Else(
                                    Assign(typeMap.TargetExpr,
                                        Convert(
                                            Call("Map",
                                                Constant(typeMap.Mapper),
                                                Convert(typeMap.SourceExpr, typeof(object)),
                                                Call("GetType", typeMap.SourceExpr),
                                                Convert(typeMap.TargetExpr, typeof(object)),
                                                Constant(typeMap.TargetExpr.Type),
                                                typeMap.BuildContextExpr),
                                            typeMap.TargetExpr.Type
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