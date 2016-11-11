using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using EntityFrameworkCore.Detached.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public class DetachedContext<TDbContext> : IDetachedContext<TDbContext>, IDisposable
        where TDbContext : DbContext
    {
        #region Fields

        TDbContext _dbContext;
        IServiceProvider _serviceProvider;
        IDbContextOptions _dbContextOptions;
        DetachedOptionsExtension _detachedOptions;
        IDetachedServices _detachedServices;

        #endregion

        #region Ctor.

        public DetachedContext()
            : this(Activator.CreateInstance<TDbContext>())
        {
        }

        public DetachedContext(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _serviceProvider = ((IInfrastructure<IServiceProvider>)_dbContext).Instance;

            // create a scope for services and plugins.
            IServiceProvider scopedProvider = _serviceProvider.GetRequiredService<IServiceScopeFactory>()
                                                              .CreateScope()
                                                              .ServiceProvider;

            // get update and load services and event manager.
            _detachedServices = scopedProvider.GetService<IDetachedServices>();
            _detachedServices.Initialize(this, scopedProvider);

            // get ef options.
            _dbContextOptions = _serviceProvider.GetService<IDbContextOptions>();
            _detachedOptions = _dbContextOptions.FindExtension<DetachedOptionsExtension>();

            // load plugins.
            _detachedServices.PluginManager.Initialize();
        }

        #endregion

        #region Properties

        DbContext IDetachedContext.DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        public TDbContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        public IEventManager Events
        {
            get
            {
                return _detachedServices.EventManager;
            }
        }

        public IPluginManager Plugins
        {
            get
            {
                return _detachedServices.PluginManager;
            }
        }

        #endregion

        public IQueryable<TEntity> GetBaseQuery<TEntity>() where TEntity : class
        {
            return _detachedServices.LoadServices.GetBaseQuery<TEntity>();
        }

        public async Task<TEntity> LoadAsync<TEntity>(params object[] key) where TEntity : class
        {
            return await _detachedServices.LoadServices.LoadAsync<TEntity>(key);
        }

        public async Task<List<TEntity>> LoadAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig) where TEntity : class
        {
            return await _detachedServices.LoadServices.LoadAsync<TEntity>(queryConfig);
        }

        public async Task<List<TResult>> LoadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TEntity : class
            where TResult : class
        {
            return await _detachedServices.LoadServices.LoadAsync<TEntity, TResult>(queryConfig);
        }

        public virtual async Task<TEntity> UpdateAsync<TEntity>(TEntity root)
            where TEntity : class
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
                persisted = (TEntity)Events.OnRootLoaded(persisted, _dbContext).Root; // entity to merge has been loaded.
                _detachedServices.UpdateServices.Merge(root, persisted); // merge existing entity.
            }
            // re-enable autodetect changes.
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

            return persisted;
        }

        public virtual async Task DeleteAsync<TEntity>(params object[] keyValues)
           where TEntity : class
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

        public virtual async Task<int> SaveChangesAsync()
        {
            // temporally disabled autodetect changes
            bool autoDetectChanges = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            int result = await _dbContext.SaveChangesAsync();

            // re-enable autodetect changes.
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
                _detachedServices.Dispose();
            }
        }
    }
}