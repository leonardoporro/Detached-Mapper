using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
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
        EventManager _eventManager = new EventManager();
        List<IDetachedPlugin> _plugins = new List<IDetachedPlugin>();

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

            // get options.
            _dbContextOptions = _serviceProvider.GetService<IDbContextOptions>();
            _detachedOptions = _dbContextOptions.FindExtension<DetachedOptionsExtension>();

            // load plugins.
            _plugins = _serviceProvider.GetServices<IDetachedPlugin>()
                                       .OrderByDescending(p => p.Priority)
                                       .ToList();

            foreach (IDetachedPlugin plugin in _plugins)
                plugin.Initialize(this);
        }

        #endregion

        #region Properties

        public TDbContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        public EventManager Events
        {
            get
            {
                return _eventManager;
            }
        }

        #endregion

        public virtual IQueryable<TEntity> GetBaseQuery<TEntity>()
            where TEntity : class
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

            // compute paths from owned and associated navigations.
            List<string> paths = new List<string>();
            GetIncludePaths(null, entityType, null, ref paths);

            // get base query.
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            // include all paths.
            foreach (string path in paths)
            {
                query = query.Include(path);
            }

            return query;
        }

        protected virtual void GetIncludePaths(IEntityType parentType, IEntityType entityType, string currentPath, ref List<string> paths)
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
                                 .Where(n => n.TargetType != parentType && (n.IsAssociated || n.IsOwned))
                                 .ToList();

            if (navs.Any())
            {
                // there are children. call recursively.
                foreach (var nav in navs)
                {
                    if (nav.IsOwned)
                        GetIncludePaths(entityType, nav.TargetType, currentPath + "." + nav.Navigation.Name, ref paths);
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
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            IKey key = entityType.FindPrimaryKey();
            Expression<Func<TEntity, bool>> keyFilter = key.CreateFindExpression<TEntity>(keyValues);
            TEntity entity = await GetBaseQuery<TEntity>()
                                        .AsNoTracking()
                                        .SingleOrDefaultAsync(keyFilter);

            return entity;
        }

        public virtual async Task<List<TEntity>> LoadAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig)
            where TEntity : class
        {
            return await LoadAsync<TEntity, TEntity>(queryConfig);
        }

        public virtual async Task<List<TResult>> LoadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TEntity : class
            where TResult : class
        {
            IQueryable<TEntity> query = GetBaseQuery<TEntity>().AsNoTracking();
            IQueryable<TResult> configQuery = queryConfig?.Invoke(query);
            List<TResult> entities = await configQuery.ToListAsync();

            return entities;
        }

        public virtual async Task<TEntity> UpdateAsync<TEntity>(TEntity root)
            where TEntity : class
        {
            // temporally disabled autodetect changes
            bool autoDetectChanges = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            IKey key = entityType.FindPrimaryKey();

            // load the persisted entity, with all the includes
            Expression<Func<TEntity, bool>> filter = key.CreateFindExpression<TEntity>(key.GetKeyValues(root));
            TEntity persisted = await GetBaseQuery<TEntity>()
                                         .AsTracking()
                                         .SingleOrDefaultAsync(filter);
            if (persisted == null)
                Add(root, null); // entity does not exist.
            else
                Merge(root, persisted, null); // entity exists.

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

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            IKey key = entityType.FindPrimaryKey();

            // load the existing entity.
            Expression<Func<TEntity, bool>> filter = key.CreateFindExpression<TEntity>(keyValues);
            TEntity persisted = await GetBaseQuery<TEntity>()
                                       .AsTracking()
                                       .SingleOrDefaultAsync(filter);

            if (persisted != null)
                Delete(persisted, null);

            // re-enable autodetect changes.
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        protected virtual EntityEntry FindEntry(object entity)
        {
            Type detachedClrType = entity.GetType();
            IEntityType entityType = _dbContext.Model.FindEntityType(detachedClrType);
            IKey key = entityType.FindPrimaryKey();

            object[] detachedKey = key.GetKeyValues(entity);
            foreach (EntityEntry entityEntry in _dbContext.ChangeTracker.Entries())
            {
                if (entityEntry.Metadata.ClrType == detachedClrType)
                {
                    object[] trackerKey = key.GetKeyValues(entityEntry.Entity);
                    bool equal = true;
                    for (int i = 0; i < trackerKey.Length; i++)
                    {
                        if (!Equals(trackerKey[i], detachedKey[i]))
                        {
                            equal = false;
                            break;
                        }
                    }

                    if (equal)
                        return entityEntry;
                }
            }

            return null;
        }

        protected virtual bool CopyProperties(object srcEntity, EntityEntry destEntry)
        {
            bool modified = false;
            foreach (PropertyEntry property in destEntry.Properties)
            {
                if (!(property.Metadata.FieldInfo == null ||
                      property.Metadata.IsKeyOrForeignKey() ||
                      property.Metadata.IsIgnored()))
                {
                    IClrPropertyGetter getter = property.Metadata.GetGetter();
                    object srcValue = getter.GetClrValue(srcEntity);
                    if (srcValue != property.CurrentValue)
                    {
                        if (!_eventManager.OnPropertyChanging(property, srcValue).Cancel)
                        {
                            property.CurrentValue = srcValue;
                            _eventManager.OnPropertyChanged(property, srcValue);
                            modified = true;
                        }
                    }
                }
            }
            return modified;
        }

        protected virtual EntityEntry Add(object detached, NavigationEntry parentNavigation)
        {
            var args = _eventManager.OnEntityAdding(detached, parentNavigation);

            EntityEntry persisted = FindEntry(args.Entity);
            if (persisted == null)
                persisted = _dbContext.Entry(args.Entity);
            persisted.State = EntityState.Added;

            foreach (NavigationEntry navigationEntry in persisted.Navigations)
            {
                IEntityType navType = navigationEntry.Metadata.GetTargetType();
                bool owned = navigationEntry.Metadata.IsOwned();
                bool associated = navigationEntry.Metadata.IsAssociated();

                IList mergedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(navType.ClrType)) as IList;
                if (navigationEntry.CurrentValue != null)
                {
                    if (navigationEntry.Metadata.IsCollection())
                    {
                        foreach (object navItem in (IEnumerable)navigationEntry.CurrentValue)
                        {
                            if (associated)
                                mergedList.Add(Attach(navItem, navigationEntry).Entity);
                            else if (owned)
                                mergedList.Add(Add(navItem, navigationEntry).Entity);
                        }
                        navigationEntry.CurrentValue = mergedList;
                    }
                    else
                    {
                        if (associated)
                            navigationEntry.CurrentValue = Attach(navigationEntry.CurrentValue, navigationEntry).Entity;
                        else if (owned)
                            navigationEntry.CurrentValue = Add(navigationEntry.CurrentValue, navigationEntry).Entity;
                    }
                }
            }

            return _eventManager.OnEntityAdded(persisted, parentNavigation).EntityEntry;
        }

        protected virtual EntityEntry Attach(object detached, NavigationEntry parentNavigation)
        {
            var args = _eventManager.OnEntityAttaching(detached, parentNavigation);

            EntityEntry persisted = FindEntry(args.Entity);
            if (persisted == null)
                persisted = _dbContext.Entry(args.Entity);
            persisted.State = EntityState.Unchanged;

            return _eventManager.OnEntityAttached(persisted, parentNavigation).EntityEntry;
        }

        protected virtual void Delete(object persisted, NavigationEntry parentNavigation)
        {
            var args = _eventManager.OnEntityDeleting(persisted, parentNavigation);

            EntityEntry entry = FindEntry(args.Entity);
            if (entry == null)
                entry = _dbContext.Entry(args.Entity);
            entry.State = EntityState.Deleted;

            foreach (NavigationEntry navigationEntry in entry.Navigations)
            {
                if (navigationEntry.Metadata.IsOwned()) //recursive deletion for owned properties.
                {
                    if (navigationEntry.CurrentValue != null)
                    {
                        if (navigationEntry.Metadata.IsCollection())
                        {
                            foreach (object item in (IEnumerable)navigationEntry.CurrentValue)
                            {
                                Delete(item, navigationEntry); //delete collection item.
                            }
                        }
                        else
                        {
                            Delete(navigationEntry.CurrentValue, navigationEntry); //delete reference.
                        }
                    }
                }
            }

            _eventManager.OnEntityDeleted(entry, parentNavigation);
        }

        protected virtual EntityEntry Merge(object detached, object persisted, NavigationEntry parentNavigation)
        {
            var args = _eventManager.OnEntityMerging(detached, persisted, parentNavigation);

            EntityEntry entry = FindEntry(args.Entity);
            if (entry == null)
                entry = _dbContext.Entry(args.Entity);

            bool modified = CopyProperties(detached, entry);

            foreach (NavigationEntry navigationEntry in entry.Navigations)
            {
                bool owned = navigationEntry.Metadata.IsOwned();
                bool associated = navigationEntry.Metadata.IsAssociated();

                if (!(associated || owned))
                    continue;

                IClrPropertyGetter getter = navigationEntry.Metadata.GetGetter();
                object detachedValue = getter.GetClrValue(detached);

                IEntityType navType = navigationEntry.Metadata.GetTargetType();
                IKey navKey = navType.FindPrimaryKey();

                if (navigationEntry.Metadata.IsCollection())
                {
                    // a mutable list to store the result.
                    IList mergedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(navType.ClrType)) as IList;

                    // create hash table for O(N) merge.
                    Dictionary<string, object> dbTable = navKey.CreateKeyEntityTable((IEnumerable)navigationEntry.CurrentValue);

                    if (detachedValue != null)
                    {
                        foreach (object detachedItem in (IEnumerable)detachedValue)
                        {
                            object persistedItem;
                            string entityKey = navKey.GetKey(detachedItem);
                            if (dbTable.TryGetValue(entityKey, out persistedItem))
                            {
                                if (owned)
                                    Merge(detachedItem, persistedItem, navigationEntry);

                                mergedList.Add(persistedItem);
                                dbTable.Remove(entityKey); // remove it from the table, to avoid deletion.
                            }
                            else
                            {
                                mergedList.Add(owned ? Add(detachedItem, navigationEntry).Entity 
                                                     : Attach(detachedItem, navigationEntry).Entity);
                            }
                        }
                    }

                    // the rest of the items in the dbTable should be removed.
                    foreach (var dbItem in dbTable)
                        Delete(dbItem.Value, navigationEntry);

                    // let EF do the rest of the work.
                    navigationEntry.CurrentValue = mergedList;
                }
                else
                {
                    if (navKey.Equal(detachedValue, navigationEntry.CurrentValue))
                    {
                        // merge owned references and do nothing for associated references.
                        if (owned)
                            navigationEntry.CurrentValue = Merge(detachedValue, navigationEntry.CurrentValue, navigationEntry).Entity;
                    }
                    else
                    {
                        if (owned)
                        {
                            if (navigationEntry.CurrentValue != null)
                                Delete(navigationEntry.CurrentValue, navigationEntry);
                        }

                        if (detachedValue != null)
                        {
                            if (owned)
                                navigationEntry.CurrentValue = Add(detachedValue, navigationEntry).Entity;
                            else
                                navigationEntry.CurrentValue = Attach(detachedValue, navigationEntry).Entity;
                        }
                        
                        navigationEntry.CurrentValue = detachedValue;
                    }
                }
            }

            return _eventManager.OnEntityMerged(detached, entry, modified, parentNavigation).EntityEntry;
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

                foreach (IDetachedPlugin plugin in _plugins)
                    plugin.Dispose();
            }
        }
    }
}