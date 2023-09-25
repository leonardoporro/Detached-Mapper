using System;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.Entity.Collection
{
    public class EntityCollectionTypeMapper<TSource, TSourceItem, TTarget, TTargetItem, TKey> : TypeMapper<TSource, TTarget>
        where TSource : IEnumerable<TSourceItem>
        where TTarget : ICollection<TTargetItem>
        where TSourceItem : class
        where TTargetItem : class
        where TKey : IEntityKey
    {
        readonly Func<TSourceItem, IMapContext, TKey> _getSourceKey;
        readonly Func<TTargetItem, IMapContext, TKey> _getTargetKey;
        readonly LazyTypeMapper<TSourceItem, TTargetItem> _itemMapper;
        readonly Func<IMapContext, TTarget> _construct;

        public EntityCollectionTypeMapper(Func<IMapContext, TTarget> construct,
                                          Func<TSourceItem, IMapContext, TKey> getSourceKey,
                                          Func<TTargetItem, IMapContext, TKey> getTargetKey,
                                          LazyTypeMapper<TSourceItem, TTargetItem> itemMapper)

        {
            _construct = construct;
            _getSourceKey = getSourceKey;
            _getTargetKey = getTargetKey;
            _itemMapper = itemMapper;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            
            Dictionary<TKey, TTargetItem> table = new Dictionary<TKey, TTargetItem>();

            if (target != null)
            {
                foreach (TTargetItem targetItem in target)
                {
                    TKey targetKey = _getTargetKey(targetItem, context);
                    table.Add(targetKey, targetItem);
                }
            }

            TTarget result = target;

            if (target == null)
                result = _construct(context);
            else
                result.Clear();

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

            if (context.Parameters.CompositeCollectionBehavior == CompositeCollectionBehavior.Append)
            {
                foreach (TTargetItem targetItem in table.Values)
                {
                    result.Add(targetItem);
                }
            }
            else
            {
                foreach (TTargetItem targetItem in table.Values)
                {
                    _itemMapper.Value.Map(null, targetItem, context);
                }
            }

            return result;
        }
    }
}