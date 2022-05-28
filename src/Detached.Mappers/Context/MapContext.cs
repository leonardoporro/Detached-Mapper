using Detached.Mappers.TypeMappers.Entity;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Detached.Mappers.Context
{
    public class MapContext : IMapContext
    {
        readonly ConcurrentDictionary<MapperContextKey, MapperContextEntry> _entries =
            new ConcurrentDictionary<MapperContextKey, MapperContextEntry>();

        readonly Dictionary<object, object> _trackedObjects = new Dictionary<object, object>();

        public MapParameters Parameters { get; } = new MapParameters();

        public virtual TEntity TrackChange<TEntity, TSource, TKey>(TEntity entity, TSource source, TKey key, MapperActionType actionType)
            where TEntity : class
            where TSource : class
            where TKey : IEntityKey
        {
            MapperContextKey mapperKey = new MapperContextKey(typeof(TEntity), key);

            _entries.AddOrUpdate(mapperKey,
                k =>
                {
                    return new MapperContextEntry { Entity = entity, ActionType = actionType };
                },
                (k, entry) =>
                {
                    entry.ActionType = actionType;
                    return entry;
                });

            return entity;
        }

        public bool TryGetEntry<TEntity>(IEntityKey key, out MapperContextEntry entry)
        {
            return _entries.TryGetValue(new MapperContextKey(typeof(TEntity), key), out entry);
        }

        public LinkedList<(object, object)> _objectStack = new LinkedList<(object, object)>();

        public virtual void PushResult<TKeyOrSource, TTarget>(TKeyOrSource keyOrSource, TTarget target)
        {
            _objectStack.AddLast((keyOrSource, target));
        }

        public virtual void PopResult()
        {
            _objectStack.RemoveLast();
        }

        public virtual bool TryGetResult<TKeyOrSource, TTarget>(TKeyOrSource keyOrDto, out TTarget target)
            where TTarget : class
        {
            var entry = _objectStack.Last;
            while (entry != null && !Equals(entry.Value.Item1, keyOrDto))
            {
                entry = entry.Previous;
            }

            if (entry != null)
            {
                target = (TTarget)entry.Value.Item2;
                return true;
            }
            else
            {
                target = default;
                return false;
            }
        }

        public virtual TTarget FindParent<TTarget>()
            where TTarget : class
        {
            var entry = _objectStack.Last;
            while (entry != null && !(entry.Value.Item1 is EntityRef entityRef && entityRef.ClrType == typeof(TTarget)))
            {
                entry = entry.Previous;
            }

            if (entry != null)
            {
                return (TTarget)entry.Value.Item2;
            }
            else
            {
                return null;
            }
        }
    }
}