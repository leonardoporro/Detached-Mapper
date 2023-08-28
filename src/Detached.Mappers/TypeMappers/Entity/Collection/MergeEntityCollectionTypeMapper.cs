using System;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.Entity.Collection
{
    public class MergeEntityCollectionTypeMapper<TSource, TSourceItem, TTarget, TTargetItem, TKey> : TypeMapper<TSource, TTarget>
        where TSource : IEnumerable<TSourceItem>
        where TTarget : ICollection<TTargetItem>
        where TSourceItem : class
        where TTargetItem : class
        where TKey : IEntityKey
    {
        readonly Func<TSourceItem, IMapContext, TKey> _getSourceKey;
        readonly Func<TTargetItem, IMapContext, TKey> _getTargetKey;
        readonly LazyTypeMapper<TSourceItem, TTargetItem> _itemMapper;
        private readonly EntityCollectionNullBehavior _entityCollectionNullBehavior;
        readonly Func<IMapContext, TTarget> _construct;

        public MergeEntityCollectionTypeMapper(Func<IMapContext, TTarget> construct,
                                          Func<TSourceItem, IMapContext, TKey> getSourceKey,
                                          Func<TTargetItem, IMapContext, TKey> getTargetKey,
                                          LazyTypeMapper<TSourceItem, TTargetItem> itemMapper,
                                          EntityCollectionNullBehavior entityCollectionNullBehavior)

        {
            _construct = construct;
            _getSourceKey = getSourceKey;
            _getTargetKey = getTargetKey;
            _itemMapper = itemMapper;
            _entityCollectionNullBehavior = entityCollectionNullBehavior;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (source == null && _entityCollectionNullBehavior == EntityCollectionNullBehavior.Ignore)
            {
                return target;
            }

            TTarget result = _construct(context);
            Dictionary<TKey, TTargetItem> table = new Dictionary<TKey, TTargetItem>();

            if (target != null)
            {
                foreach (TTargetItem targetItem in target)
                {
                    TKey targetKey = _getTargetKey(targetItem, context);
                    table.Add(targetKey, targetItem);
                }
            }

            if (source != null)
            {
                foreach (TSourceItem sourceItem in source)
                {
                    TKey sourceKey = _getSourceKey(sourceItem, context);

                    if (table.TryGetValue(sourceKey, out TTargetItem targetItem))
                    {
                        table.Remove(sourceKey);
                    }

                    result.Add(_itemMapper.Value.Map(sourceItem, targetItem, context));
                }
            }

            foreach (TTargetItem targetItem in table.Values)
            {
                result.Add(targetItem);
            }

            return result;
        }
    }
}