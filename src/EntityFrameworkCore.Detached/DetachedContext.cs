using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Exceptions;
using EntityFrameworkCore.Detached.Plugins;
using EntityFrameworkCore.Detached.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public class DetachedContext<TDbContext> : IDetachedContext<TDbContext>, IDisposable
        where TDbContext : DbContext
    {
        #region Fields

        TDbContext _dbContext;
        IServiceProvider _serviceProvider;
        IDetachedContextServices _detachedServices;

        ConcurrentDictionary<Type, IDetachedSet> _setCache = new ConcurrentDictionary<Type, IDetachedSet>();
        ConcurrentDictionary<string, Type> _setNameCache = new ConcurrentDictionary<string, Type>();
        
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

            // get update and load services and event manager.
            _detachedServices = _serviceProvider.GetService<IDetachedContextServices>();
            if (_detachedServices == null)
                throw new DetachedSetupException(_dbContext.GetType());

            // set THIS as the current detached context.
            _detachedServices.SetDetachedContext(this);

            // load plugins.
            Plugins.Initialize();
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

        public DetachedOptionsExtension Options
        {
            get
            {
                return _detachedServices.DetachedOptions;
            }
        }

        #endregion

        public IDetachedSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return (IDetachedSet<TEntity>)Set(typeof(TEntity));
        }

        public IDetachedSet Set(Type entityType)
        {
            return _setCache.GetOrAdd(entityType, t =>
            {
                Type setType = typeof(IDetachedSet<>).MakeGenericType(entityType);
                IDetachedSet detachedSet = (IDetachedSet)_detachedServices.ServiceProvider.GetRequiredService(setType);
                return detachedSet;
            });
        }

        public IDetachedSet Set(string propertyName, bool throwIfNotFound = true)
        {
            Type setType = _setNameCache.GetOrAdd(propertyName, n =>
            {
                PropertyInfo propInfo = _dbContext.GetType()
                                                  .GetRuntimeProperties()
                                                  .Where(p => string.Equals(p.Name, n, StringComparison.CurrentCultureIgnoreCase))
                                                  .FirstOrDefault();
                                
                if (propInfo == null)
                {
                    if (throwIfNotFound)
                        throw new ArgumentException($"Property {propertyName} does not exist in {_dbContext.GetType().Name}.");
                    else
                        return null;
                }

                TypeInfo propTypeInfo = propInfo.PropertyType.GetTypeInfo();
                if (!(propTypeInfo.IsGenericType && propTypeInfo.GetGenericTypeDefinition() == typeof(DbSet<>)))
                {
                    if (throwIfNotFound)
                        throw new ArgumentException($"Property {propertyName} of DbContext {_dbContext.GetType().Name} is not of DbSet<> type.");
                    else
                        return null;
                }

                Type entityType = propTypeInfo.GenericTypeArguments.First();
                return entityType;
            });

            return Set(setType);
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