using System.Collections.Concurrent;

namespace Detached.Mapping.Context
{
    public class MapperContext : IMapperContext
    {
        readonly ConcurrentDictionary<MapperContextKey, MapperContextEntry> _entries =
            new ConcurrentDictionary<MapperContextKey, MapperContextEntry>();

        public MapperOptions MapperOptions { get; } = new MapperOptions();

        public TEntity OnMapperAction<TEntity, TSource, TKey>(TEntity entity, TSource source, TKey key, MapperActionType actionType)
            where TEntity : class
            where TSource : class
            where TKey : IEntityKey
        {
            MapperContextKey mapperKey = new MapperContextKey(typeof(TEntity), key);

            _entries.AddOrUpdate(mapperKey,
                key =>
                {
                    return new MapperContextEntry { Entity = entity, ActionType = actionType };
                },
                (key, entry) =>
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