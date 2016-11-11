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

namespace EntityFrameworkCore.Detached.Events
{
    public class EventManager : IEventManager
    {
        public EventManager()
        {
        }

        WeakEvent<EntityAttachingEventArgs> _entityAttaching = new WeakEvent<EntityAttachingEventArgs>();
        public event EventHandler<EntityAttachingEventArgs> EntityAttaching
        {
            add
            {
                _entityAttaching.Subscribe(value);
            }
            remove
            {
                _entityAttaching.Unsubscribe(value);
            }
        }

        WeakEvent<EntityAttachedEventArgs> _entityAttached = new WeakEvent<EntityAttachedEventArgs>();
        public event EventHandler<EntityAttachedEventArgs> EntityAttached
        {
            add
            {
                _entityAttached.Subscribe(value);
            }
            remove
            {
                _entityAttached.Unsubscribe(value);
            }
        }

        WeakEvent<EntityAddingEventArgs> _entityAdding = new WeakEvent<EntityAddingEventArgs>();
        public event EventHandler<EntityAddingEventArgs> EntityAdding
        {
            add
            {
                _entityAdding.Subscribe(value);
            }
            remove
            {
                _entityAdding.Unsubscribe(value);
            }
        }

        WeakEvent<EntityAddedEventArgs> _entityAdded = new WeakEvent<EntityAddedEventArgs>();
        public event EventHandler<EntityAddedEventArgs> EntityAdded
        {
            add
            {
                _entityAdded.Subscribe(value);
            }
            remove
            {
                _entityAdded.Unsubscribe(value);
            }
        }

        WeakEvent<EntityDeletingEventArgs> _entityDeleting = new WeakEvent<EntityDeletingEventArgs>();
        public event EventHandler<EntityDeletingEventArgs> EntityDeleting
        {
            add
            {
                _entityDeleting.Subscribe(value);
            }
            remove
            {
                _entityDeleting.Unsubscribe(value);
            }
        }

        WeakEvent<EntityDeletedEventArgs> _entityDeleted = new WeakEvent<EntityDeletedEventArgs>();
        public event EventHandler<EntityDeletedEventArgs> EntityDeleted
        {
            add
            {
                _entityDeleted.Subscribe(value);
            }
            remove
            {
                _entityDeleted.Unsubscribe(value);
            }
        }

        WeakEvent<EntityMergingEventArgs> _entityMerging = new WeakEvent<EntityMergingEventArgs>();
        public event EventHandler<EntityMergingEventArgs> EntityMerging
        {
            add
            {
                _entityMerging.Subscribe(value);
            }
            remove
            {
                _entityMerging.Unsubscribe(value);
            }
        }

        WeakEvent<EntityMergedEventArgs> _entityMerged = new WeakEvent<EntityMergedEventArgs>();
        public event EventHandler<EntityMergedEventArgs> EntityMerged
        {
            add
            {
                _entityMerged.Subscribe(value);
            }
            remove
            {
                _entityMerged.Unsubscribe(value);
            }
        }

        WeakEvent<EntityLoadedEventArgs> _entityLoaded = new WeakEvent<EntityLoadedEventArgs>();
        public event EventHandler<EntityLoadedEventArgs> EntityLoaded
        {
            add
            {
                _entityLoaded.Subscribe(value);
            }
            remove
            {
                _entityLoaded.Unsubscribe(value);
            }
        }

        WeakEvent<PropertyChangingEventArgs> _propertyChanging = new WeakEvent<PropertyChangingEventArgs>();
        public event EventHandler<PropertyChangingEventArgs> PropertyChanging
        {
            add
            {
                _propertyChanging.Subscribe(value);
            }
            remove
            {
                _propertyChanging.Unsubscribe(value);
            }
        }

        WeakEvent<PropertyChangedEventArgs> _propertyChanged = new WeakEvent<PropertyChangedEventArgs>();
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged
        {
            add
            {
                _propertyChanged.Subscribe(value);
            }
            remove
            {
                _propertyChanged.Unsubscribe(value);
            }
        }

        WeakEvent<RootLoadingEventArgs> _rootLoading = new WeakEvent<RootLoadingEventArgs>();
        public event EventHandler<RootLoadingEventArgs> RootLoading
        {
            add
            {
                _rootLoading.Subscribe(value);
            }
            remove
            {
                _rootLoading.Unsubscribe(value);
            }
        }

        WeakEvent<RootLoadedEventArgs> _rootLoaded = new WeakEvent<RootLoadedEventArgs>();
        public event EventHandler<RootLoadedEventArgs> RootLoaded
        {
            add
            {
                _rootLoaded.Subscribe(value);
            }
            remove
            {
                _rootLoaded.Unsubscribe(value);
            }
        }

        public EntityAddingEventArgs OnEntityAdding(object detached, NavigationEntry navigation)
        {
            EntityAddingEventArgs args = new EntityAddingEventArgs
            {
                Entity = detached,
                ParentNavigationEntry = navigation
            };
            _entityAdding.Raise(this, args);
            return args;
        }

        public EntityAddedEventArgs OnEntityAdded(EntityEntry persisted, NavigationEntry navigation)
        {
            EntityAddedEventArgs args = new EntityAddedEventArgs
            {
                EntityEntry = persisted,
                ParentNavigationEntry = navigation
            };
            _entityAdded.Raise(this, args);
            return args;
        }

        public EntityAttachingEventArgs OnEntityAttaching(object detached, NavigationEntry navigation)
        {
            EntityAttachingEventArgs args = new EntityAttachingEventArgs
            {
                Entity = detached,
                ParentNavigationEntry = navigation
            };
            _entityAttaching.Raise(this, args);

            return args;
        }

        public EntityAttachedEventArgs OnEntityAttached(EntityEntry persisted, NavigationEntry navigation)
        {
            EntityAttachedEventArgs args = new EntityAttachedEventArgs
            {
                EntityEntry = persisted,
                ParentNavigationEntry = navigation
            };
            _entityAttached.Raise(this, args);
            return args;
        }

        public EntityDeletingEventArgs OnEntityDeleting(object persisted, NavigationEntry navigation)
        {
            EntityDeletingEventArgs args = new EntityDeletingEventArgs
            {
                Entity = persisted,
                ParentNavigationEntry = navigation
            };
            _entityDeleting.Raise(this, args);
            return args;
        }

        public EntityDeletedEventArgs OnEntityDeleted(EntityEntry entry, NavigationEntry navigation)
        {
            EntityDeletedEventArgs args = new EntityDeletedEventArgs
            {
                EntityEntry = entry,
                ParentNavigationEntry = navigation
            };
            _entityDeleted.Raise(this, args);
            return args;
        }

        public EntityMergingEventArgs OnEntityMerging(object detached, object persisted, NavigationEntry navigation)
        {
            EntityMergingEventArgs args = new EntityMergingEventArgs
            {
                DetachedEntity = detached,
                Entity = persisted,
                ParentNavigationEntry = navigation
            };
            _entityMerging.Raise(this, args);
            return args;
        }

        public EntityMergedEventArgs OnEntityMerged(object detached, EntityEntry entry, bool modified, NavigationEntry navigation)
        {
            EntityMergedEventArgs args = new EntityMergedEventArgs
            {
                DetachedEntity = detached,
                EntityEntry = entry,
                Modified = modified,
                ParentNavigationEntry = navigation
            };
            _entityMerged.Raise(this, args);
            return args;
        }

        public PropertyChangingEventArgs OnPropertyChanging(PropertyEntry property, object srcValue)
        {
            PropertyChangingEventArgs args = new PropertyChangingEventArgs
            {
                Property = property,
                NewValue = srcValue,
            };
            _propertyChanging.Raise(this, args);
            return args;
        }

        public PropertyChangedEventArgs OnPropertyChanged(PropertyEntry property, object srcValue)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs
            {
                Property = property,
                OldValue = srcValue
            };
            _propertyChanged.Raise(this, args);
            return args;
        }

        public RootLoadingEventArgs OnRootLoading(IQueryable queryable, DbContext dbContext)
        {
            RootLoadingEventArgs args = new RootLoadingEventArgs
            {
                Queryable = queryable
            };
            _rootLoading.Raise(this, args);
            return args;
        }

        public RootLoadedEventArgs OnRootLoaded(object root, DbContext dbContext)
        {
            RootLoadedEventArgs args = new RootLoadedEventArgs
            {
                Root = root
            };

            if (root != null)
            {
                _rootLoaded.Raise(this, args);

                IEntityType entityType = dbContext.Model.FindEntityType(root.GetType());
                if (_entityLoaded.HasSubscribers) // do not raise if nobody is listenting.
                    OnEntityLoaded(entityType, root, null, new HashSet<object>());
            }

            return args;
        }

        public void OnEntityLoaded(IEntityType entityType, object entity, INavigation parentNavigation, HashSet<object> visited)
        {
            visited.Add(entity);
            EntityLoadedEventArgs args = new EntityLoadedEventArgs
            {
                Entity = entity,
                EntityType = entityType,
                ParentNavigation = parentNavigation
            };
            _entityLoaded.Raise(this, args);

            foreach (INavigation navigation in entityType.GetNavigations())
            {
                IEntityType itemType = navigation.GetTargetType();
                if (navigation.IsCollection())
                {
                    IEnumerable collection = navigation.GetGetter().GetClrValue(entity) as IEnumerable;
                    if (collection != null)
                    {
                        foreach (object item in collection)
                        {
                            OnEntityLoaded(itemType, item, navigation, visited);
                        }
                    }
                }
                else
                {
                    object reference = navigation.GetGetter().GetClrValue(entity);
                    if (reference != null && !visited.Contains(reference))
                    {
                        OnEntityLoaded(itemType, reference, navigation, visited);
                    }
                }
            }
        }
    }
}
