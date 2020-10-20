using Detached.Mappers.TypeMaps;
using System.Collections.Concurrent;

namespace Detached.Mappers.Context
{
    public class MapperContext : IMapperContext
    {
        readonly ConcurrentDictionary<MapperContextKey, MapperContextEntry> _entries =
            new ConcurrentDictionary<MapperContextKey, MapperContextEntry>();

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

        public bool TryGetEntry<TEntity>(IEntityKey key, out MapperContextEntry entry)
        {
            return _entries.TryGetValue(new MapperContextKey(typeof(TEntity), key), out entry);
        }
    }
}