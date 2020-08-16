using AgileObjects.ReadableExpressions;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers
{
    public class EntityCollectionMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsCollection
                && typeMap.ItemMap.SourceOptions.IsComplex
                && typeMap.TargetOptions.IsCollection
                && typeMap.ItemMap.TargetOptions.IsEntity;
        }

        public override Delegate Create(TypeMap typeMap)
        {
            Type sourceType = typeMap.SourceOptions.Type;
            Type targetType = typeMap.TargetOptions.Type;

            LambdaExpression mapExpr =
                Lambda(
                    Parameter("source_" + sourceType.Name, sourceType, out Expression source),
                    Parameter("target_" + targetType.Name, targetType, out Expression target),
                    Parameter("context", typeof(IMapperContext), out Expression context),
                    MergeCollection(typeMap, source, target, context),
                    Result(target)
                );

            Debug.WriteLine(mapExpr.ToReadableString());

            return mapExpr.Compile();
        }

        public Expression MergeCollection(TypeMap typeMap, Expression source, Expression target, Expression context)
        {
            ParameterExpression sourceItem = Parameter(typeMap.ItemMap.SourceOptions.Type, "sourceItem");
            ParameterExpression targetItem = Parameter(typeMap.ItemMap.TargetOptions.Type, "targetItem");

            MapKey(typeMap.ItemMap, sourceItem, targetItem, context, out Expression sourceId, out Expression targetId);

            Expression itemMap = Map(typeMap.ItemMap, sourceItem, targetItem, context);

            return If(IsNotNull(source),
                Then(
                    Variable(sourceItem),
                    Variable(targetItem),
                    // copy source values to hash table.
                    Variable("table", New(DictionaryOf(sourceId.Type, sourceItem.Type)), out Expression sourceTable),
                    ForEach(
                        sourceItem,
                        In(source),
                        Call("Add", sourceTable, sourceId, sourceItem)
                    ),

                    // if target is null, create.
                    If(IsNull(target), Assign(target, New(target.Type))),

                    // iterate target replace or remove items.
                    Variable("i", Subtract(Property(target, "Count"), Constant(1)), out Expression i),
                    For(
                        GreaterThanOrEqual(i, Constant(0)),
                        PostDecrementAssign(i),
                        Block(
                            Assign(targetItem, Index(target, i)),
                            Variable("key", targetId, out Expression key),
                            If(
                                Call("TryGetValue", sourceTable, key, sourceItem),
                                Then(
                                    Variable("mapped", itemMap, out Expression mapped),
                                    Assign(Index(target, i), mapped),
                                    Call("Remove", sourceTable, key)
                                ),
                                Else(
                                    itemMap,
                                    Call("RemoveAt", target, i)
                                )
                            )
                        )
                    ),
                    // iterate items left in hash table and add.
                    Variable("entry", KeyValueOf(targetId.Type, sourceItem.Type), out Expression entry),
                    ForEach(
                        entry,
                        In(sourceTable),
                        Block(
                            Assign(targetItem, Default(targetItem.Type)),
                            Assign(sourceItem, Property(entry, "Value")),
                            Call("Add", target, itemMap)
                        )
                    )
                ),
                Else(
                    Assign(target, Default(target.Type))
                )
            );
        }
    }
}