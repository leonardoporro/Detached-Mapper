using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers.Entity
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
                    Parameter(typeMap.Source),
                    Parameter(typeMap.Target),
                    Parameter(typeMap.Context),
                    Block(
                        Variable(itemMap.Source),
                        Variable(itemMap.Target),

                        CreateMemberMappers(typeMap.ItemMap),
                        CreateKey(itemMap),
                        CreateMapper(typeMap.ItemMap),

                        If(IsNull(typeMap.Source),
                            Then(
                                SetToDefault(typeMap.Target)
                            ),
                            Else(
                                // if target is null, create.
                                If(IsNull(typeMap.Target),
                                    Assign(typeMap.Target, typeMap.TargetOptions.Construct(typeMap.Context))
                                ),

                                // copy source values to hash table.
                                Variable("mergeTable", New(DictionaryOf(itemMap.SourceKey.Type, itemMap.Source.Type)), out Expression mergeTable),
                                Variable("addList", New(ListOf(itemMap.Source.Type)), out Expression addList),
                                ForEach(
                                    itemMap.Source,
                                    In(typeMap.Source),
                                    IfThenElse(IsNotNull(itemMap.SourceKey),
                                        Call("Add", mergeTable, itemMap.SourceKey, itemMap.Source),
                                        Call("Add", addList, itemMap.Source)
                                    )
                                ),
                                // iterate target replace or remove items.
                                Variable("i", Subtract(Property(typeMap.Target, "Count"), Constant(1)), out Expression i),
                                For(
                                    GreaterThanOrEqual(i, Constant(0)),
                                    PostDecrementAssign(i),
                                    Block(
                                        Assign(itemMap.Target, Index(typeMap.Target, i)),
                                        Variable("id", itemMap.TargetKey, out Expression id),
                                        If(Call("TryGetValue", mergeTable, id, itemMap.Source),
                                            Then(
                                                Assign(Index(typeMap.Target, i), CallMapper(itemMap, itemMap.Source, itemMap.Target)),
                                                Call("Remove", mergeTable, id)
                                            ),
                                            Else(
                                                CallMapper(itemMap, Default(itemMap.Source.Type), itemMap.Target),
                                                Call("RemoveAt", typeMap.Target, i)
                                            )
                                        )
                                    )
                                ),
                                // iterate items left in hash table and add.
                                Variable("entry", KeyValueOf(itemMap.TargetKey.Type, itemMap.Source.Type), out Expression entry),
                                ForEach(
                                    entry,
                                    In(mergeTable),
                                    Call("Add", typeMap.Target, CallMapper(itemMap, Property(entry, "Value"), Default(itemMap.Target.Type)))
                                ),
                                // iterate items with null key and add
                                Variable("item", itemMap.Source.Type, out Expression item),
                                ForEach(
                                    item,
                                    In(addList),
                                    Call("Add", typeMap.Target, CallMapper(itemMap, item, Default(itemMap.Target.Type)))
                                )
                            )
                        ),
                        Result(typeMap.Target)
                    )
                );
        }
    }
}