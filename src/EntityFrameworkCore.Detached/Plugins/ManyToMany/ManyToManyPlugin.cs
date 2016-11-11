using EntityFrameworkCore.Detached.Events;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.Detached.Plugins.ManyToMany
{
    public class ManyToManyPlugin : DetachedPlugin
    {
        Dictionary<Type, List<ManyToManyMetadata>> _metadataCache = new Dictionary<Type, List<ManyToManyMetadata>>();
        IEventManager _eventManager;

        public ManyToManyPlugin(IEventManager eventManager, IModel model)
        {
            _eventManager = eventManager;
         
            string annotationKey = typeof(ManyToManyMetadata).FullName;
            foreach (IEntityType entityType in model.GetEntityTypes())
            {
                var annotation = entityType.FindAnnotation(annotationKey);
                if (annotation != null)
                {
                    _metadataCache[entityType.ClrType] = (List<ManyToManyMetadata>)annotation.Value;
                }
            }
        }

        protected override void OnEnabled()
        {
            _eventManager.EntityAdding += Events_EntityAdding;
            _eventManager.EntityMerging += Events_EntityMerging;
            _eventManager.EntityLoaded += Events_EntityLoaded;
            IsEnabled = true;
        }

        protected override void OnDisabled()
        {
            _eventManager.EntityAdding -= Events_EntityAdding;
            _eventManager.EntityMerging -= Events_EntityMerging;
            _eventManager.EntityLoaded -= Events_EntityLoaded;
            IsEnabled = false;
        }

        private void Events_EntityLoaded(object sender, EntityLoadedEventArgs e)
        {
            CopyToCollection(e.Entity);
        }

        private void Events_EntityMerging(object sender, EntityMergingEventArgs e)
        {
            CopyToTable(e.Entity);
            CopyToTable(e.DetachedEntity);
        }

        private void Events_EntityAdding(object sender, EntityAddingEventArgs e)
        {
            CopyToTable(e.Entity);
        }

        void CopyToCollection(object entity)
        {
            List<ManyToManyMetadata> metadataList;
            if (_metadataCache.TryGetValue(entity.GetType(), out metadataList))
            {
                foreach (ManyToManyMetadata metadata in metadataList)
                {
                    metadata.ToCollection(entity);
                }
            }
        }

        void CopyToTable(object entity)
        {
            List<ManyToManyMetadata> metadataList;
            if (_metadataCache.TryGetValue(entity.GetType(), out metadataList))
            {
                foreach (ManyToManyMetadata metadata in metadataList)
                {
                    metadata.ToTable(entity);
                }
            }
        }
    }
}
