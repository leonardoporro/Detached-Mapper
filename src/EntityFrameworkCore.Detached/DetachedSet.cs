using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public class DetachedSet<TEntity> : IDetachedSet<TEntity>, IDetachedSet
        where TEntity : class
    {
        #region Fields

        IDetachedContextServices _detachedServices;
        IEventManager _eventManager;
        DbContext _dbContext;

        #endregion

        #region Ctor.

        public DetachedSet(IDetachedContextServices detachedServices, IEventManager eventManager)
        {
            _detachedServices = detachedServices;
            _dbContext = detachedServices.DetachedContext.DbContext;
            _eventManager = eventManager;
        }

        #endregion

        public Type EntityType
        {
            get
            {
                return typeof(TEntity);
            }
        }

        public IQueryable<TEntity> GetBaseQuery()
        {
            return _detachedServices.LoadServices.GetBaseQuery<TEntity>();
        }

        public async Task<TEntity> LoadAsync(params object[] key)
        {
            return await _detachedServices.LoadServices.LoadAsync<TEntity>(key);
        }

        public async Task<List<TEntity>> LoadAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig)
        {
            return await _detachedServices.LoadServices.LoadAsync<TEntity>(queryConfig);
        }

        public async Task<List<TResult>> LoadAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TResult : class
        {
            return await _detachedServices.LoadServices.LoadAsync<TEntity, TResult>(queryConfig);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity root)
        {
            // temporally disabled autodetect changes
            bool autoDetectChanges = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            TEntity persisted = await _detachedServices.LoadServices.LoadPersisted<TEntity>(root);
            if (persisted == null) // add new entity.
            {
                persisted = (TEntity)_detachedServices.UpdateServices.Add(root).Entity;
            }
            else
            {
                persisted = (TEntity)_eventManager.OnRootLoaded(persisted, _dbContext).Root; // entity to merge has been loaded.
                _detachedServices.UpdateServices.Merge(root, persisted); // merge existing entity.
            }
            // re-enable autodetect changes.
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

            return persisted;
        }

        public virtual async Task DeleteAsync(params object[] keyValues)
        {
            // temporally disable autodetect changes.
            bool autoDetectChanges = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            TEntity persisted = await _detachedServices.LoadServices.LoadPersisted<TEntity>(keyValues);
            if (persisted != null)
                _detachedServices.UpdateServices.Delete(persisted);

            // re-enable autodetect changes.
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        IQueryable IDetachedSet.GetBaseQuery()
        {
            return GetBaseQuery();
        }

        async Task<object> IDetachedSet.LoadAsync(params object[] key)
        {
            return await LoadAsync(key);
        }

        public async Task<object> UpdateAsync(object root)
        {
            return await UpdateAsync((TEntity)root);
        }
    }
}