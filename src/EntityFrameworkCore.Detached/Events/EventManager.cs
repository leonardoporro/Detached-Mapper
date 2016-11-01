using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Events
{
    public class EventManager
    {
        IDetachedContext detachedContext;

        public event EventHandler<EntityAttachingEventArgs> EntityAttaching;
        public event EventHandler<EntityAttachedEventArgs> EntityAttached;
        public event EventHandler<EntityAddingEventArgs> EntityAdding;
        public event EventHandler<EntityAddedEventArgs> EntityAdded;
        public event EventHandler<EntityDeletingEventArgs> EntityDeleting;
        public event EventHandler<EntityDeletedEventArgs> EntityDeleted;
        public event EventHandler<EntityMergingEventArgs> EntityMerging;
        public event EventHandler<EntityMergedEventArgs> EntityMerged;
        public event EventHandler<PropertyChangingEventArgs> PropertyChanging;
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        public EntityAddingEventArgs OnEntityAdding(object detached, NavigationEntry navigation)
        {
            EntityAddingEventArgs args = new EntityAddingEventArgs
            {
                Entity = detached,
                ParentNavigationEntry = navigation
            };
            EntityAdding?.Invoke(detachedContext, args);
            return args;
        }

        public EntityAddedEventArgs OnEntityAdded(EntityEntry persisted, NavigationEntry navigation)
        {
            EntityAddedEventArgs args = new EntityAddedEventArgs
            {
                EntityEntry = persisted,
                ParentNavigationEntry = navigation
            };
            EntityAdded?.Invoke(detachedContext, args);
            return args;
        }

        public EntityAttachingEventArgs OnEntityAttaching(object detached, NavigationEntry navigation)
        {
            EntityAttachingEventArgs args = new EntityAttachingEventArgs
            {
                Entity = detached,
                ParentNavigationEntry = navigation
            };
            EntityAttaching?.Invoke(detachedContext, args);
            return args;
        }

        public EntityAttachedEventArgs OnEntityAttached(EntityEntry persisted, NavigationEntry navigation)
        {
            EntityAttachedEventArgs args = new EntityAttachedEventArgs
            {
                EntityEntry = persisted,
                ParentNavigationEntry = navigation
            };
            EntityAttached?.Invoke(detachedContext, args);
            return args;
        }

        public EntityDeletingEventArgs OnEntityDeleting(object persisted, NavigationEntry navigation)
        {
            EntityDeletingEventArgs args = new EntityDeletingEventArgs
            {
                Entity = persisted,
                ParentNavigationEntry = navigation
            };
            EntityDeleting?.Invoke(detachedContext, args);
            return args;
        }

        public EntityDeletedEventArgs OnEntityDeleted(EntityEntry entry, NavigationEntry navigation)
        {
            EntityDeletedEventArgs args = new EntityDeletedEventArgs
            {
                EntityEntry = entry,
                ParentNavigationEntry = navigation
            };
            EntityDeleted?.Invoke(detachedContext, args);
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
            EntityMerging?.Invoke(detachedContext, args);
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
            EntityMerged?.Invoke(detachedContext, args);
            return args;
        }

        public PropertyChangingEventArgs OnPropertyChanging(PropertyEntry property, object srcValue)
        {
            PropertyChangingEventArgs args = new PropertyChangingEventArgs
            {
                Property = property,
                NewValue = srcValue,
            };
            PropertyChanging?.Invoke(detachedContext, args);
            return args;
        }

        public PropertyChangedEventArgs OnPropertyChanged(PropertyEntry property, object srcValue)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs
            {
                Property = property,
                OldValue = srcValue
            };
            PropertyChanged?.Invoke(detachedContext, args);
            return args;
        }
    }
}
