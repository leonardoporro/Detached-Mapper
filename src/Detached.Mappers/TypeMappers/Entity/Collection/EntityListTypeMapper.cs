using System;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.Entity.Collection
{
    public class EntityListTypeMapper<TSource, TSourceItem, TTarget, TTargetItem, TKey> : TypeMapper<TSource, TTarget>
        where TSource : IEnumerable<TSourceItem>
        where TTarget : IList<TTargetItem>
        where TTargetItem : class
        where TKey : IEntityKey
    {
        readonly Func<TSourceItem, IMapContext, TKey> _getSourceKey;
        readonly Func<TTargetItem, IMapContext, TKey> _getTargetKey;
        readonly ITypeMapper<TSourceItem, TTargetItem> _itemMapper;
        readonly Func<IMapContext, TTarget> _construct;

        public EntityListTypeMapper(Func<IMapContext, TTarget> construct,
                                    Func<TSourceItem, IMapContext, TKey> getSourceKey,
                                    Func<TTargetItem, IMapContext, TKey> getTargetKey,
                                    ITypeMapper<TSourceItem, TTargetItem> itemMapper)

        {
            _construct = construct;
            _getSourceKey = getSourceKey;
            _getTargetKey = getTargetKey;
            _itemMapper = itemMapper;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            Dictionary<TKey, TTargetItem> table = new Dictionary<TKey, TTargetItem>();

            if (target == null)
            {
                target = _construct(context);
            }

            foreach (TTargetItem targetItem in target)
            {
                TKey targetKey = _getTargetKey(targetItem, context);
                table.Add(targetKey, targetItem);
            }

            if (source != null)
            {
                foreach (TSourceItem sourceItem in source)
                {
                    TKey sourceKey = _getSourceKey(sourceItem, context);

                    if (table.TryGetValue(sourceKey, out TTargetItem targetItem))
                    {
                        _itemMapper.Map(sourceItem, targetItem, context);
                        table.Remove(sourceKey);
                    }
                    else
                    {
                        target.Add(_itemMapper.Map(sourceItem, null, context));
                    }
                }
            }

            if (context.Parameters.CompositeCollectionBehavior != CompositeCollectionBehavior.Append)
            {
                for (int i = target.Count - 1; i >= 0; i--)
                {
                    TTargetItem targetItem = target[i];
                    TKey targetKey = _getTargetKey(targetItem, context);

                    if (table.ContainsKey(targetKey))
                    {
                        _itemMapper.Map(null, targetItem, context);
                        target.RemoveAt(i);
                    }
                }
            }

            return target;
        }
    }
}