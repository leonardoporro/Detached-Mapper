using Detached.Mappers.EntityFramework.Queries;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMappers.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapContext : MapContext
    {
        public EntityMapContext(
            EntityMapper mapper,
            DbContext dbContext, 
            QueryProvider queryProvider,  
            MapParameters parameters)
            : base(parameters)
        {
            Mapper = mapper;
            QueryProvider = queryProvider;
            DbContext = dbContext; 
        }

        public EntityMapper Mapper { get; }

        public QueryProvider QueryProvider { get; }

        public DbContext DbContext { get; } 

        public override TTarget TrackChange<TTarget, TSource, TKey>(TTarget entity, TSource source, TKey key, MapperActionType actionType)
        {
            if (actionType == MapperActionType.Load)
            {
                if (!key.IsEmpty)
                {
                    QueryProvider.Load(DbContext.Set<TTarget>(), source);
                }

                TTarget loadedEntity = GetExistingEntry<TTarget, TKey>(key)?.Entity;

                if (loadedEntity == null && !Parameters.Upsert)
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
                            if (Parameters.AddAggregations)
                            {
                                entry.Reload();
                                if (entry.State == EntityState.Detached)
                                {
                                    entry.State = EntityState.Added;
                                }
                            }
                            else
                            {
                                entry.State = EntityState.Unchanged;
                            }
                            break;
                        case MapperActionType.Create:
                            if (!key.IsEmpty && Parameters.AssociateExistingCompositions)
                            {
                                entry.Reload();
                            }
                            if (entry.State == EntityState.Detached)
                            {
                                entry.State = EntityState.Added;
                            }
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
                        case MapperActionType.Update:

                            if (Mapper.ConcurrencyTokens.TryGetValue(entry.Metadata.Name, out string concurrencyTokenName))
                            {
                                PropertyEntry concurrencyTokenProperty = entry.Property(concurrencyTokenName);
                                concurrencyTokenProperty.OriginalValue = concurrencyTokenProperty.CurrentValue;
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
