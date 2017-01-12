using Detached.EntityFramework.Events;
using Detached.EntityFramework.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Services
{
    public class UpdateServices : IUpdateServices
    {
        #region Fields

        IEventManager _eventManager;
        IEntryFinder _entryServices;
        IEntityServicesFactory _keyServicesFactory;
        DbContext _dbContext;
        DetachedOptionsExtension _options;

        #endregion

        #region Ctor.

        public UpdateServices(DbContext dbContext,
                              IEntryFinder entryServices,
                              IEntityServicesFactory keyServicesFactory,
                              IEventManager eventManager,
                              DetachedOptionsExtension options)
        {
            _dbContext = dbContext;
            _eventManager = eventManager;
            _entryServices = entryServices;
            _keyServicesFactory = keyServicesFactory;
            _options = options;
        }

        #endregion

        public virtual bool Copy(object srcEntity, EntityEntry destEntry)
        {
            bool modified = false;
            foreach (PropertyEntry property in destEntry.Properties)
            {
                if (!(property.Metadata.FieldInfo == null ||
                      property.Metadata.IsPrimaryKey() ||
                      property.Metadata.IsForeignKey() ||
                      property.Metadata.IsStoreGeneratedAlways ||
                      property.Metadata.IsReadOnlyAfterSave ||
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

        public EntityEntry Add(object detached)
        {
            return Add(detached, null, new HashSet<object>());
        }

        public EntityEntry Attach(object detached)
        {
            return Attach(detached, null);
        }

        public void Delete(object persisted)
        {
            Delete(persisted, null, new HashSet<object>());
        }

        public EntityEntry Merge(object detached, object persisted)
        {
            return Merge(detached, persisted, null, new HashSet<object>());
        }

        protected virtual EntityEntry Add(object detached, NavigationEntry parentNavigation, HashSet<object> visited)
        {
            var args = _eventManager.OnEntityAdding(detached, parentNavigation);

            // set state.
            EntityEntry persisted = GetEntry(args.Entity);
            persisted.State = EntityState.Added;
            visited.Add(persisted.Entity);

            // trying to add an entity that has a primary key defined?. 
            if (persisted.IsKeySet
                && _options.ThrowExceptionOnEntityNotFound
                && persisted.Properties.Where(p => p.Metadata.IsKey()).Any(p => p.Metadata.ValueGenerated != ValueGenerated.Never))
            {
                throw new EntityNotFoundException
                {
                    EntityType = persisted.Metadata.ClrType,
                    KeyValues = persisted.Properties.Where(p => p.Metadata.IsKey()).Select(p => p.CurrentValue).ToArray()
                };
            }

            // recurse through navigations.
            foreach (NavigationEntry navigationEntry in persisted.Navigations)
            {
                IEntityType navType = navigationEntry.Metadata.GetTargetType();
                bool owned = navigationEntry.Metadata.IsOwned();
                bool associated = navigationEntry.Metadata.IsAssociated();

                if (!(associated || owned))
                    continue;

                if (navigationEntry.CurrentValue != null)
                {
                    if (navigationEntry.Metadata.IsCollection())
                    {
                        // create a 3rd list object to update change tracker and also avoid read-only collection exceptions.
                        IList mergedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(navType.ClrType)) as IList;

                        foreach (object navItem in (IEnumerable)navigationEntry.CurrentValue)
                        {
                            if (associated)
                                mergedList.Add(Attach(navItem, navigationEntry).Entity);
                            else if (owned)
                                mergedList.Add(Add(navItem, navigationEntry, visited).Entity);
                        }

                        // let EF Core do the work.
                        navigationEntry.CurrentValue = mergedList;
                    }
                    else
                    {
                        if (!visited.Contains(navigationEntry.CurrentValue))
                        {
                            // recurse and let EF Core do the work.
                            if (associated)
                                navigationEntry.CurrentValue = Attach(navigationEntry.CurrentValue, navigationEntry).Entity;
                            else if (owned)
                                navigationEntry.CurrentValue = Add(navigationEntry.CurrentValue, navigationEntry, visited).Entity;
                        }
                    }
                }
            }

            return _eventManager.OnEntityAdded(persisted, parentNavigation).EntityEntry;
        }

        protected virtual EntityEntry Attach(object detached, NavigationEntry parentNavigation)
        {
            var args = _eventManager.OnEntityAttaching(detached, parentNavigation);

            EntityEntry persisted = GetEntry(detached);
            if (persisted.State != EntityState.Added && persisted.State != EntityState.Deleted)
                persisted.State = EntityState.Unchanged;

            return _eventManager.OnEntityAttached(persisted, parentNavigation).EntityEntry;
        }

        protected virtual void Delete(object persisted, NavigationEntry parentNavigation, HashSet<object> visited)
        {
            var args = _eventManager.OnEntityDeleting(persisted, parentNavigation);

            EntityEntry entry = GetEntry(args.Entity);
            entry.State = EntityState.Deleted;
            visited.Add(entry.Entity);

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
                                Delete(item, navigationEntry, visited); //delete collection item.
                            }
                        }
                        else
                        {
                            if (!visited.Contains(navigationEntry.CurrentValue))
                                Delete(navigationEntry.CurrentValue, navigationEntry, visited); //delete reference.
                        }
                    }
                }
            }

            _eventManager.OnEntityDeleted(entry, parentNavigation);
        }

        protected virtual EntityEntry Merge(object detached, object persisted, NavigationEntry parentNavigation, HashSet<object> visited)
        {
            var args = _eventManager.OnEntityMerging(detached, persisted, parentNavigation);

            EntityEntry persistedEntry = _entryServices.FindEntry(persisted);
            if (persistedEntry == null)
                persistedEntry = _dbContext.Entry(persisted);

            visited.Add(persistedEntry.Entity);

            bool modified = Copy(detached, persistedEntry);

            foreach (NavigationEntry navigationEntry in persistedEntry.Navigations)
            {
                bool owned = navigationEntry.Metadata.IsOwned();
                bool associated = navigationEntry.Metadata.IsAssociated();

                if (!(associated || owned))
                    continue;

                IEntityType navType = navigationEntry.Metadata.GetTargetType();
                IClrPropertyGetter getter = navigationEntry.Metadata.GetGetter();
                object detachedValue = getter.GetClrValue(detached);

                IEntityServices keyServices = _keyServicesFactory.GetEntityServices(navType);

                if (navigationEntry.Metadata.IsCollection())
                {
                    // a mutable list to store the result.
                    IList mergedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(navType.ClrType)) as IList;

                    // create hash table for O(N) merge.
                    Dictionary<object[], object> dbTable = keyServices.CreateTable((IEnumerable)navigationEntry.CurrentValue);

                    if (detachedValue != null)
                    {
                        foreach (object detachedItem in (IEnumerable)detachedValue)
                        {
                            object persistedItem;
                            object[] entityKey = keyServices.GetKeyValues(detachedItem);
                            if (dbTable.TryGetValue(entityKey, out persistedItem))
                            {
                                if (owned)
                                    mergedList.Add(Merge(detachedItem, persistedItem, navigationEntry, visited).Entity);
                                else
                                    mergedList.Add(persistedItem);

                                dbTable.Remove(entityKey); // remove it from the table, to avoid deletion.
                            }
                            else
                            {
                                mergedList.Add(owned ? Add(detachedItem, navigationEntry, visited).Entity
                                                     : Attach(detachedItem, navigationEntry).Entity);
                            }
                        }
                    }

                    // the rest of the items in the dbTable should be removed.
                    foreach (var dbItem in dbTable)
                        Delete(dbItem.Value, navigationEntry, visited);

                    // let EF do the rest of the work.
                    navigationEntry.CurrentValue = mergedList;
                }
                else
                {
                    if (!visited.Contains(navigationEntry.CurrentValue)) // avoid stack overflow! (this might be also done checking if the property is dependent to parent)
                    {
                        if (keyServices.Equal(detachedValue, navigationEntry.CurrentValue))
                        {
                            // merge owned references and do nothing for associated references.
                            if (owned)
                                navigationEntry.CurrentValue = Merge(detachedValue, navigationEntry.CurrentValue, navigationEntry, visited).Entity;
                        }
                        else
                        {
                            if (navigationEntry.CurrentValue != null)
                            {
                                if (owned)
                                    Delete(navigationEntry.CurrentValue, navigationEntry, visited);
                            }

                            if (detachedValue != null)
                            {
                                if (owned)
                                    navigationEntry.CurrentValue = Add(detachedValue, navigationEntry, visited).Entity;
                                else
                                    navigationEntry.CurrentValue = Attach(detachedValue, navigationEntry).Entity;
                            }
                            else
                            {
                                navigationEntry.CurrentValue = null;
                            }
                        }
                    }
                }
            }

            return _eventManager.OnEntityMerged(detached, persistedEntry, modified, parentNavigation).EntityEntry;
        }

        EntityEntry GetEntry(object entity)
        {
            EntityEntry entry = _entryServices.FindEntry(entity);
            if (entry == null)
            {
                entry = _dbContext.Entry(entity);
                Copy(entity, entry);
            }
            return entry;
        }
    }
}