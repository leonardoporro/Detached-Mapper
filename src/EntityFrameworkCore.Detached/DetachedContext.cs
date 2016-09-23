using EntityFrameworkCore.Detached.Contracts;
using EntityFrameworkCore.Detached.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public class DetachedContext<TDbContext> : IDetachedContext<TDbContext>
        where TDbContext : DbContext
    {
        #region Fields

        TDbContext _dbContext;
        IDetachedQueryManager _queryManager;
        IDetachedUpdateManager _updateManager;

        #endregion

        #region Ctor.

        /// <summary>
        /// Initializes a new instance of DetachedContext.
        /// </summary>
        /// <param name="context">The base DbContext instance.</param>
        /// <param name="queryManager">The manager for db queries.</param>
        /// <param name="updateManager">The manager for db updates.</param>
        public DetachedContext(TDbContext dbContext)
            : this(dbContext, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of DetachedContext.
        /// </summary>
        /// <param name="context">The base DbContext instance.</param>
        /// <param name="queryManager">The manager for db queries.</param>
        /// <param name="updateManager">The manager for db updates.</param>
        public DetachedContext(TDbContext dbContext, IDetachedSessionInfoProvider sessionInfoProvider)
        {
            _dbContext = dbContext;
            _queryManager = new ManyToManyQueryManager(dbContext);
            _updateManager = new ManyToManyUpdateManager(dbContext, sessionInfoProvider);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying DbContext instance.
        /// </summary>
        public TDbContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        #endregion

        public virtual async Task<TEntity> LoadAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            EntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity)) as EntityType;
            return await _queryManager.FindEntityByKey<TEntity>(entityType, keyValues);
        }

        public virtual async Task<List<TEntity>> LoadAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class
        {
            EntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity)) as EntityType;
            return await _queryManager.FindEntities<TEntity>(entityType, filter);
        }

        public virtual async Task<TEntity> SaveAsync<TEntity>(TEntity root)
            where TEntity : class
        {
            // temporally disabled autodetect changes
            bool autoDetectChanges = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            EntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity)) as EntityType;
            if (entityType == null)
                throw new ArgumentException($"{typeof(TEntity)} is not a valid entity.");

            // load the persisted entity, with all the includes
            TEntity dbEntity = await _queryManager.FindPersistedEntity<TEntity>(entityType, root);

            if (dbEntity == null)
                _updateManager.Add(entityType, root); // entity does not exist.
            else
                _updateManager.Merge(entityType, root, dbEntity); // entity exists.

            // re-enable autodetect changes.
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

            return dbEntity;
        }

        public virtual async Task DeleteAsync<TEntity>(TEntity root)
            where TEntity : class
        {
            EntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity)) as EntityType;
            _updateManager.Delete(entityType, root);
            await _dbContext.SaveChangesAsync();
        }
    }
}