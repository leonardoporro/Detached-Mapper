using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class ObjectToObjectMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.Source.Type == typeof(object) && typeMap.Target.Type == typeof(object);
        }
        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.Source),
                        Parameter(typeMap.Target),
                        Parameter(typeMap.Context),
                        Block(
                            If(IsNull(typeMap.Source),
                                Then(
                                    Assign(typeMap.Target, Default(typeMap.Target.Type))
                                ),
                                Else(
                                    If(IsNotNull(typeMap.Target),
                                        Then(
                                            Assign(typeMap.Target,
                                                Convert(
                                                    Call("Map",
                                                        Constant(typeMap.Mapper),
                                                        Convert(typeMap.Source, typeof(object)),
                                                        Call("GetType", typeMap.Source),
                                                        Convert(typeMap.Target, typeof(object)),
                                                        Call("GetType", typeMap.Target),
                                                        typeMap.Context),
                                                    typeMap.Target.Type
                                                )
                                            )
                                        ),
                                        Else(
                                            Assign(typeMap.Target, typeMap.Source)
                                        )
                                    )
                                )
                            ),
                            Result(typeMap.Target)
                        )
                    );
        }
    }
}