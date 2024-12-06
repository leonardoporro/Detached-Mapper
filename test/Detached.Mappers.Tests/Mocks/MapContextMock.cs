using Detached.Mappers.Context;
using Detached.Mappers.TypeMappers.Entity;
using System.Collections.Concurrent;

namespace Detached.Mappers.Tests.Mocks
{
    public class MapContextMock : MapContext
    {
        readonly ConcurrentDictionary<MapContextKey, MapContextEntry> _entries =
            new ConcurrentDictionary<MapContextKey, MapContextEntry>();

        public MapContextMock(MapParameters parameters = null)
            : base(parameters)
        {
        }

        public override TEntity TrackChange<TEntity, TSource, TKey>(TEntity entity, TSource source, TKey key, MapperActionType actionType)
        {
            MapContextKey mapperKey = new MapContextKey(typeof(TEntity), key);

            _entries.AddOrUpdate(mapperKey,
                k => new MapContextEntry { Entity = entity, ActionType = actionType },
                (k, entry) =>
                {
                    entry.ActionType = actionType;
                    return entry;
                });

            return entity;
        }

        public bool Verify<TEntity>(IEntityKey key, out MapContextEntry entry)
        {
            return _entries.TryGetValue(new MapContextKey(typeof(TEntity), key), out entry);
        }
    }
}