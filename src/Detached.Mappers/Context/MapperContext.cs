using Detached.Mappers.TypeMaps;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Detached.Mappers.Context
{
    public class MapperContext : IMapperContext
    {
        readonly ConcurrentDictionary<MapperContextKey, MapperContextEntry> _entries =
            new ConcurrentDictionary<MapperContextKey, MapperContextEntry>();

        readonly Dictionary<object, object> _trackedObjects = new Dictionary<object, object>();

        public MapperParameters Parameters { get; } = new MapperParameters();

        public TEntity OnMapperAction<TEntity, TSource, TKey>(TEntity entity, TSource source, TKey key, MapperActionType actionType)
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

        public bool TryGetTrackedObject<TKeyOrSource, TTarget>(TKeyOrSource keyOrDto, out TTarget target)
        {
            if (_trackedObjects.TryGetValue(keyOrDto, out var entry))
            {
                target = (TTarget)entry;
                return true;
            }
            else
            {
                target = default;
                return false;
            }
        }

        public void TrackObject<TKeyOrSource, TTarget>(TKeyOrSource keyOrSource, TTarget target)
        {
            _trackedObjects.Add(keyOrSource, target);
        }

        public bool TryGetEntry<TEntity>(IEntityKey key, out MapperContextEntry entry)
        {
            return _entries.TryGetValue(new MapperContextKey(typeof(TEntity), key), out entry);
        }
    }
}