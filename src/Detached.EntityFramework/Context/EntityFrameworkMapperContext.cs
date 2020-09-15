using Detached.EntityFramework.Queries;
using Detached.Mapping;
using Detached.Mapping.Context;
using Detached.Mapping.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.EntityFramework.Context
{
    public class EntityFrameworkMapperContext : IMapperContext
    {
        public EntityFrameworkMapperContext(DbContext dbContext, QueryProvider queryProvider, MapperOptions mapperOptions)
        {
            QueryProvider = queryProvider;
            MapperOptions = mapperOptions;
            DbContext = dbContext;
        }

        public MapperOptions MapperOptions { get; }

        public QueryProvider QueryProvider { get; }

        public DbContext DbContext { get; }

        public TTarget OnMapperAction<TTarget, TSource, TKey>(TTarget entity, TSource source, TKey key, MapperActionType actionType)
            where TTarget : class
            where TSource : class
            where TKey : IEntityKey
        {
            if (actionType == MapperActionType.Load)
            {
                TTarget loadedEntity = QueryProvider.Load(DbContext.Set<TTarget>(), source);
                if (loadedEntity == null && !MapperOptions.Upsert)
                {
                    throw new MapperException($"Entity {typeof(TTarget)} with key [{string.Join(", ", key.ToObject())}] does not exist.");
                }
                return loadedEntity;
            }
            else
            {
                IStateManager stateManager = DbContext.GetService<IStateManager>();

                IEntityType entityType = DbContext.Model.FindEntityType(typeof(TTarget));
                IKey keyType = entityType.FindPrimaryKey();

                InternalEntityEntry internalEntry = stateManager.TryGetEntry(keyType, key.ToObject());
                EntityEntry<TTarget> entry;

                bool detached = internalEntry == null;

                if (detached)
                    entry = DbContext.Entry(entity);
                else
                    entry = new EntityEntry<TTarget>(internalEntry);

                switch (actionType)
                {
                    case MapperActionType.Attach:
                        if (detached)
                        {
                            entry.State = EntityState.Unchanged;
                        }
                        break;
                    case MapperActionType.Create:
                        entry.State = EntityState.Added;
                        break;
                    case MapperActionType.Delete:
                        entry.State = EntityState.Deleted;
                        break;
                    case MapperActionType.Update:
                        // Do nothing, as change tracking should detect the changes.
                        break;
                }

                return entry.Entity;
            }
        }
    }
}
