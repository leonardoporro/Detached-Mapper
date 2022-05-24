using Detached.Mappers.TypeMaps;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories.Entity
{
    public class EntityListMapperFactory : EntityMapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceTypeOptions.IsCollectionType
                && typeMap.TargetTypeOptions.IsCollectionType
                && typeMap.ItemTypeMap.TargetTypeOptions.IsEntityType
                && typeMap.ItemTypeMap.SourceTypeOptions.IsComplexType;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            TypeMap itemMap = typeMap.ItemTypeMap;

            return Lambda(
                    GetDelegateType(typeMap),
                    Parameter(typeMap.SourceExpression),
                    Parameter(typeMap.TargetExpression),
                    Parameter(typeMap.BuildContextExpression),
                    Block(
                        Variable(itemMap.SourceExpression),
                        Variable(itemMap.TargetExpression),

                        CreateMemberMappers(typeMap.ItemTypeMap),
                        CreateKey(itemMap),
                        CreateMapper(typeMap.ItemTypeMap),

                        If(IsNull(typeMap.SourceExpression),
                            Then(
                                SetToDefault(typeMap.TargetExpression)
                            ),
                            Else(
                                // if target is null, create.
                                If(IsNull(typeMap.TargetExpression),
                                    Assign(typeMap.TargetExpression, Construct(typeMap))
                                ),

                                // copy source values to hash table.
                                Variable("mergeTable", New(DictionaryOf(itemMap.SourceKeyExpression.Type, itemMap.SourceExpression.Type)), out Expression mergeTable),
                                Variable("addList", New(ListOf(itemMap.SourceExpression.Type)), out Expression addList),
                                ForEach(
                                    itemMap.SourceExpression,
                                    In(typeMap.SourceExpression),
                                    IfThenElse(IsNotNull(itemMap.SourceKeyExpression),
                                        Call("Add", mergeTable, itemMap.SourceKeyExpression, itemMap.SourceExpression),
                                        Call("Add", addList, itemMap.SourceExpression)
                                    )
                                ),
                                // iterate target replace or remove items.
                                Variable("i", Subtract(Property(typeMap.TargetExpression, "Count"), Constant(1)), out Expression i),
                                For(
                                    GreaterThanOrEqual(i, Constant(0)),
                                    PostDecrementAssign(i),
                                    Block(
                                        Assign(itemMap.TargetExpression, Index(typeMap.TargetExpression, i)),
                                        Variable("id", itemMap.TargetKeyExpression, out Expression id),
                                        If(Call("TryGetValue", mergeTable, id, itemMap.SourceExpression),
                                            Then(
                                                Assign(Index(typeMap.TargetExpression, i), CallMapper(itemMap, itemMap.SourceExpression, itemMap.TargetExpression)),
                                                Call("Remove", mergeTable, id)
                                            ),
                                            Else(
                                                CallMapper(itemMap, Default(itemMap.SourceExpression.Type), itemMap.TargetExpression),
                                                Call("RemoveAt", typeMap.TargetExpression, i)
                                            )
                                        )
                                    )
                                ),
                                // iterate items left in hash table and add.
                                Variable("entry", KeyValueOf(itemMap.TargetKeyExpression.Type, itemMap.SourceExpression.Type), out Expression entry),
                                ForEach(
                                    entry,
                                    In(mergeTable),
                                    Call("Add", typeMap.TargetExpression, CallMapper(itemMap, Property(entry, "Value"), Default(itemMap.TargetExpression.Type)))
                                ),
                                // iterate items with null key and add
                                Variable("item", itemMap.SourceExpression.Type, out Expression item),
                                ForEach(
                                    item,
                                    In(addList),
                                    Call("Add", typeMap.TargetExpression, CallMapper(itemMap, item, Default(itemMap.TargetExpression.Type)))
                                )
                            )
                        ),
                        Result(typeMap.TargetExpression)
                    )
                );
        }
    }
}