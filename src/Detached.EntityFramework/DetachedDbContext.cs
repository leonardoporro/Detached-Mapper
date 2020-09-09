using Detached.EntityFramework.Convention;
using Detached.EntityFramework.Queries;
using Detached.Mapping;
using Detached.Mapping.Context;
using Detached.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework
{
    public class DetachedDbContext : DbContext, IMapperContext
    {
        public DetachedDbContext(DbContextOptions options)
            : base(options)
        {
            ModelOptions modelOptions = new ModelOptions();
            modelOptions.Conventions.Add(new IsEntityConvention(this));
            OnMapperCreating(modelOptions);
            Mapper = new Mapper(Options.Create(modelOptions), new TypeMapFactory());
            QueryProvider = new QueryProvider(Mapper);
        }

        public DetachedDbContext(DbContextOptions options, Mapper mapper, QueryProvider queryProvider)
            : base(options)
        {
            Mapper = mapper;
            QueryProvider = queryProvider;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return QueryProvider.Project<TProjection, TEntity>(query);
        }

        protected virtual Mapper Mapper { get; }

        protected virtual QueryProvider QueryProvider { get; }

        protected virtual void OnMapperCreating(ModelOptions options)
        {
        }

        public Task<TEntity> MapAsync<TEntity>(object entityOrDTO)
            where TEntity : class
        {
            return Task.Run(() => Map<TEntity>(entityOrDTO));
        }

        public TEntity Map<TEntity>(object entityOrDTO)
            where TEntity : class
        {
            return (TEntity)Mapper.Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), this);
        }

        public TEntity OnMapperAction<TEntity, TSource, TKey>(TEntity entity, TSource source, TKey key, MapperActionType actionType)
            where TEntity : class
            where TSource : class
            where TKey : IEntityKey
        {
            if (actionType == MapperActionType.Load)
            {
                return QueryProvider.Load(Set<TEntity>(), source);
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