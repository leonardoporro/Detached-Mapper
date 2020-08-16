using Detached.EntityFramework.Queries;
using Detached.Mapping;
using Detached.Mapping.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Detached.EntityFramework
{
    public class DetachedDbContext : DbContext, IMapperContext
    {
        readonly Mapper _mapper;
        readonly QueryProvider _queryProvider;

        public DetachedDbContext(DbContextOptions options, Mapper mapper, QueryProvider queryProvider)
            : base(options)
        {
            _mapper = mapper;
            _queryProvider = queryProvider;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return _queryProvider.Project<TProjection, TEntity>(query);
        } 

        public async Task<TEntity> MapAsync<TEntity>(object entityOrDTO)
            where TEntity : class
        {
            TEntity mapped = (TEntity)_mapper.Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), this);
            return mapped;
        }

        public TEntity OnMapperAction<TEntity, TSource, TKey>(TEntity entity, TSource source, TKey key, MapperActionType actionType)
            where TEntity : class
            where TSource : class
            where TKey : IEntityKey
        {
            if (actionType == MapperActionType.Load)
            {
                return _queryProvider.Load(Set<TEntity>(), source);
            }
            else
            {
                IStateManager stateManager = this.GetService<IStateManager>();

                IEntityType entityType = Model.FindEntityType(typeof(TEntity));
                IKey keyType = entityType.FindPrimaryKey();

                InternalEntityEntry internalEntry = stateManager.TryGetEntry(keyType, key.ToObject());
                EntityEntry<TEntity> entry;
                if (internalEntry != null)
                    entry = new EntityEntry<TEntity>(internalEntry);
                else
                    entry = Entry(entity);

                switch (actionType)
                {
                    case MapperActionType.Attach:
                        entry.State = EntityState.Unchanged;
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