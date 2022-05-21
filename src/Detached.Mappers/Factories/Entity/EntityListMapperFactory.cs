using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories.Entity
{
    public class EntityListMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsCollection
                && typeMap.TargetOptions.IsCollection
                && typeMap.ItemMap.TargetOptions.IsEntity
                && typeMap.ItemMap.SourceOptions.IsComplexType;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            TypeMap itemMap = typeMap.ItemMap;

            return Lambda(
                    GetDelegateType(typeMap),
                    Parameter(typeMap.SourceExpr),
                    Parameter(typeMap.TargetExpr),
                    Parameter(typeMap.BuildContextExpr),
                    Block(
                        Variable(itemMap.SourceExpr),
                        Variable(itemMap.TargetExpr),

                        CreateMemberMappers(typeMap.ItemMap),
                        CreateKey(itemMap),
                        CreateMapper(typeMap.ItemMap),

                        If(IsNull(typeMap.SourceExpr),
                            Then(
                                SetToDefault(typeMap.TargetExpr)
                            ),
                            Else(
                                // if target is null, create.
                                If(IsNull(typeMap.TargetExpr),
                                    Assign(typeMap.TargetExpr, Construct(typeMap))
                                ),

                                // copy source values to hash table.
                                Variable("mergeTable", New(DictionaryOf(itemMap.SourceKeyExpr.Type, itemMap.SourceExpr.Type)), out Expression mergeTable),
                                Variable("addList", New(ListOf(itemMap.SourceExpr.Type)), out Expression addList),
                                ForEach(
                                    itemMap.SourceExpr,
                                    In(typeMap.SourceExpr),
                                    IfThenElse(IsNotNull(itemMap.SourceKeyExpr),
                                        Call("Add", mergeTable, itemMap.SourceKeyExpr, itemMap.SourceExpr),
                                        Call("Add", addList, itemMap.SourceExpr)
                                    )
                                ),
                                // iterate target replace or remove items.
                                Variable("i", Subtract(Property(typeMap.TargetExpr, "Count"), Constant(1)), out Expression i),
                                For(
                                    GreaterThanOrEqual(i, Constant(0)),
                                    PostDecrementAssign(i),
                                    Block(
                                        Assign(itemMap.TargetExpr, Index(typeMap.TargetExpr, i)),
                                        Variable("id", itemMap.TargetKeyExpr, out Expression id),
                                        If(Call("TryGetValue", mergeTable, id, itemMap.SourceExpr),
                                            Then(
                                                Assign(Index(typeMap.TargetExpr, i), CallMapper(itemMap, itemMap.SourceExpr, itemMap.TargetExpr)),
                                                Call("Remove", mergeTable, id)
                                            ),
                                            Else(
                                                CallMapper(itemMap, Default(itemMap.SourceExpr.Type), itemMap.TargetExpr),
                                                Call("RemoveAt", typeMap.TargetExpr, i)
                                            )
                                        )
                                    )
                                ),
                                // iterate items left in hash table and add.
                                Variable("entry", KeyValueOf(itemMap.TargetKeyExpr.Type, itemMap.SourceExpr.Type), out Expression entry),
                                ForEach(
                                    entry,
                                    In(mergeTable),
                                    Call("Add", typeMap.TargetExpr, CallMapper(itemMap, Property(entry, "Value"), Default(itemMap.TargetExpr.Type)))
                                ),
                                // iterate items with null key and add
                                Variable("item", itemMap.SourceExpr.Type, out Expression item),
                                ForEach(
                                    item,
                                    In(addList),
                                    Call("Add", typeMap.TargetExpr, CallMapper(itemMap, item, Default(itemMap.TargetExpr.Type)))
                                )
                            )
                        ),
                        Result(typeMap.TargetExpr)
                    )
                );
        }
    }
}