using Detached.Mappers.Context;
using Detached.Mappers.EntityFramework.Queries;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMaps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.Mappers.EntityFramework.Context
{
    public class EntityFrameworkMapperContext : IMapperContext
    {
        public EntityFrameworkMapperContext(DbContext dbContext, DetachedQueryProvider queryProvider, MapperParameters parameters)
        {
            QueryProvider = queryProvider;
            Parameters = parameters;
            DbContext = dbContext;
        }

        public MapperParameters Parameters { get; }

        public DetachedQueryProvider QueryProvider { get; }

        public DbContext DbContext { get; }

        public TTarget OnMapperAction<TTarget, TSource, TKey>(TTarget entity, TSource source, TKey key, MapperActionType actionType)
            where TTarget : class
            where TSource : class
            where TKey : IEntityKey
        {
            if (actionType == MapperActionType.Load)
            {
                if (!key.IsEmpty)
                {
                    QueryProvider.Load(DbContext.Set<TTarget>(), source);
                }

                TTarget loadedEntity = GetExistingEntry<TTarget, TKey>(key)?.Entity;

                if (loadedEntity == null && !Parameters.RootUpsert)
                {
                    throw new MapperException($"Entity {typeof(TTarget)} with key [{string.Join(", ", key.ToObject())}] does not exist.");
                }

                return loadedEntity;
            }
            else
            {
                EntityEntry<TTarget> entry = GetExistingEntry<TTarget, TKey>(key);
                if (entry == null)
                {
                    entry = DbContext.Entry(entity);

                    switch (actionType)
                    {
                        case MapperActionType.Attach:
                            if (Parameters.AggregationAction == AggregationAction.Map)
                            {
                                entry.Reload();
                                if (entry.State == EntityState.Detached)
                                    entry.State = EntityState.Added;
                            }
                            else
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
                    }
                }
                else
                {
                    switch (actionType)
                    {
                        case MapperActionType.Create:
                            entry.State = EntityState.Added;
                            break;
                        case MapperActionType.Delete:
                            entry.State = EntityState.Deleted;
                            break;
                    }
                }

                return entry.Entity;
            }
        }

        public EntityEntry<TEntity> GetExistingEntry<TEntity, TKey>(TKey key)
            where TEntity : class
            where TKey : IEntityKey
        {
            if (typeof(TKey) == typeof(NoKey))
                return null;

            IStateManager stateManager = DbContext.GetService<IStateManager>();
            IEntityType entityType = DbContext.Model.FindEntityType(typeof(TEntity));
            IKey keyType = entityType.FindPrimaryKey();

            InternalEntityEntry internalEntry = stateManager.TryGetEntry(keyType, key.ToObject());
            if (internalEntry != null)
                return new EntityEntry<TEntity>(internalEntry);
            else
                return null;
        }
    }
}
