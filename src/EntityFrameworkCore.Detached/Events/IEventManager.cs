using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Detached.Events
{
    public interface IEventManager
    {
        event EventHandler<EntityAddedEventArgs> EntityAdded;
        event EventHandler<EntityAddingEventArgs> EntityAdding;
        event EventHandler<EntityAttachedEventArgs> EntityAttached;
        event EventHandler<EntityAttachingEventArgs> EntityAttaching;
        event EventHandler<EntityDeletedEventArgs> EntityDeleted;
        event EventHandler<EntityDeletingEventArgs> EntityDeleting;
        event EventHandler<EntityLoadedEventArgs> EntityLoaded;
        event EventHandler<EntityMergedEventArgs> EntityMerged;
        event EventHandler<EntityMergingEventArgs> EntityMerging;
        event EventHandler<PropertyChangedEventArgs> PropertyChanged;
        event EventHandler<PropertyChangingEventArgs> PropertyChanging;
        event EventHandler<RootLoadedEventArgs> RootLoaded;
        event EventHandler<RootLoadingEventArgs> RootLoading;

        EntityAddedEventArgs OnEntityAdded(EntityEntry persisted, NavigationEntry navigation);
        EntityAddingEventArgs OnEntityAdding(object detached, NavigationEntry navigation);
        EntityAttachedEventArgs OnEntityAttached(EntityEntry persisted, NavigationEntry navigation);
        EntityAttachingEventArgs OnEntityAttaching(object detached, NavigationEntry navigation);
        EntityDeletedEventArgs OnEntityDeleted(EntityEntry entry, NavigationEntry navigation);
        EntityDeletingEventArgs OnEntityDeleting(object persisted, NavigationEntry navigation);
        void OnEntityLoaded(IEntityType entityType, object entity, INavigation parentNavigation, HashSet<object> visited);
        EntityMergedEventArgs OnEntityMerged(object detached, EntityEntry entry, bool modified, NavigationEntry navigation);
        EntityMergingEventArgs OnEntityMerging(object detached, object persisted, NavigationEntry navigation);
        PropertyChangedEventArgs OnPropertyChanged(PropertyEntry property, object srcValue);
        PropertyChangingEventArgs OnPropertyChanging(PropertyEntry property, object srcValue);
        RootLoadedEventArgs OnRootLoaded(object root, DbContext dbContext);
        RootLoadingEventArgs OnRootLoading(IQueryable queryable, DbContext dbContext);
    }
}