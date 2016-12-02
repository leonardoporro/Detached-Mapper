using EntityFrameworkCore.Detached.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Services
{
    public class LoadServices : ILoadServices
    {
        #region Fields

        IEventManager _eventManager;
        IKeyServicesFactory _keyServicesFactory;
        DbContext _dbContext;

        #endregion

        #region Ctor.

        public LoadServices(DbContext dbContext,
                            IEventManager eventManager,
                            IKeyServicesFactory keyServicesFactory)
        {
            _eventManager = eventManager;
            _keyServicesFactory = keyServicesFactory;
            _dbContext = dbContext;
        }

        #endregion

        public virtual IQueryable<TEntity> GetBaseQuery<TEntity>()
            where TEntity : class
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

            // compute paths from owned and associated navigations.
            List<string> paths = new List<string>();
            GetIncludePaths(entityType, new HashSet<IEntityType>(), null, ref paths);

            // get base query.
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            // include all paths.
            foreach (string path in paths)
            {
                query = query.Include(path);
            }

            return query;
        }

        public virtual void GetIncludePaths(IEntityType entityType, HashSet<IEntityType> visited, string currentPath, ref List<string> paths)
        {
            // gets a list of navigations.
            var navs = entityType.GetNavigations()
                                 .Select(n => new
                                 {
                                     Navigation = n,
                                     IsOwned = n.IsOwned(),
                                     IsAssociated = n.IsAssociated(),
                                     TargetType = n.GetTargetType()
                                 })
                                 .Where(n => (n.IsAssociated || n.IsOwned))
                                 .ToList();

            visited.Add(entityType);

            if (navs.Any())
            {
                // there are children. call recursively.
                foreach (var nav in navs)
                {
                    bool recurse = !visited.Contains(nav.TargetType);
                    if (nav.IsOwned && recurse)
                        GetIncludePaths(nav.TargetType, visited, currentPath + "." + nav.Navigation.Name, ref paths);
                    else
                        paths.Add((currentPath + "." + nav.Navigation.Name).TrimStart('.'));
                }
            }
            else if (currentPath != null)
            {
                // no more children. path ends here.
                paths.Add(currentPath.TrimStart('.'));
            }
        }

        public virtual async Task<TEntity> LoadAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            IKeyServices<TEntity> keyServices = _keyServicesFactory.GetKeyServices<TEntity>();
            Expression<Func<TEntity, bool>> keyFilter = keyServices.CreateEqualityExpression(keyValues);

            IQueryable<TEntity> query = GetBaseQuery<TEntity>().AsNoTracking();
            query = (IQueryable<TEntity>)_eventManager.OnRootLoading(query, _dbContext).Queryable;

            TEntity entity = await query.SingleOrDefaultAsync(keyFilter);
            if (entity != null)
                _eventManager.OnRootLoaded(entity, _dbContext);

            return entity;
        }

        public virtual async Task<List<TResult>> LoadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TEntity : class
            where TResult : class
        {
            IQueryable<TEntity> query = GetBaseQuery<TEntity>().AsNoTracking();
            IQueryable<TResult> configQuery = queryConfig?.Invoke(query);

            configQuery = (IQueryable<TResult>)_eventManager.OnRootLoading(configQuery, _dbContext).Queryable;

            List<TResult> entities = await configQuery.ToListAsync();
            for (int i = 0; i < entities.Count; i++)
                entities[i] = (TResult)_eventManager.OnRootLoaded(entities[i], _dbContext).Root;

            return entities;
        }

        public virtual async Task<List<TEntity>> LoadAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig)
            where TEntity : class
        {
            return await LoadAsync<TEntity, TEntity>(queryConfig);
        }

        public async Task<TEntity> LoadPersisted<TEntity>(TEntity entity)
            where TEntity : class
        {
            IKeyServices<TEntity> keyServices = _keyServicesFactory.GetKeyServices<TEntity>();
            return await LoadPersisted<TEntity>(keyServices.GetValues(entity));
        }

        public async Task<TEntity> LoadPersisted<TEntity>(object[] keyValues)
            where TEntity : class
        {
            IKeyServices<TEntity> keyServices = _keyServicesFactory.GetKeyServices<TEntity>();
            Expression<Func<TEntity, bool>> keyFilter = keyServices.CreateEqualityExpression(keyValues);

            IQueryable<TEntity> query = GetBaseQuery<TEntity>().AsTracking().Where(keyFilter);
            query = (IQueryable<TEntity>)_eventManager.OnRootLoading(query, _dbContext).Queryable;

            TEntity persisted = await query.SingleOrDefaultAsync();
            if (persisted != null)
                persisted = (TEntity)_eventManager.OnRootLoaded(persisted, _dbContext).Root;

            return persisted;
        }
    }
}