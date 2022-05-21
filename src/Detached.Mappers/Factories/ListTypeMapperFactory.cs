using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class ListMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceTypeOptions.IsCollection
                && typeMap.TargetTypeOptions.IsCollection;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.SourceExpression),
                        Parameter(typeMap.TargetExpression),
                        Parameter(typeMap.BuildContextExpression),
                        Block(
                            CreateMapper(typeMap.ItemTypeMap),
                            If(IsNull(typeMap.SourceExpression),
                                Then(
                                    Assign(typeMap.TargetExpression, Default(typeMap.TargetExpression.Type))
                                ),
                                Else(
                                    Assign(typeMap.TargetExpression, Construct(typeMap)),
                                    Variable(typeMap.ItemTypeMap.SourceExpression),
                                    ForEach(
                                        typeMap.ItemTypeMap.SourceExpression,
                                        In(typeMap.SourceExpression),
                                        Block(
                                            Call("Add",
                                                typeMap.TargetExpression,
                                                CallMapper(
                                                    typeMap.ItemTypeMap,
                                                    typeMap.ItemTypeMap.SourceExpression,
                                                    Default(typeMap.ItemTypeMap.TargetExpression.Type)
                                                )
                                            )
                                        )
                                    )
                                )
                            ),
                            Result(typeMap.TargetExpression)
                        )
                    );
        }

        public Expression Construct(TypeMap typeMap)
        {
            return typeMap.TargetTypeOptions.Construct(typeMap.BuildContextExpression, null);
        }
    }
}