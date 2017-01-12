using Detached.EntityFramework.Events;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Plugins.Auditing
{
    public class AuditingPlugin : DetachedPlugin
    {
        ISessionInfoProvider _sessionInfoProvider;
        IEventManager _eventManager;

        public AuditingPlugin(ISessionInfoProvider sessionInfoProvider,
                              IEventManager eventManager)
        {
            _sessionInfoProvider = sessionInfoProvider;
            _eventManager = eventManager;
        }

        protected override void OnEnabled()
        {
            _eventManager.EntityAdded += Events_EntityAdded;
            _eventManager.EntityMerged += Events_EntityMerged;
        }

        protected override void OnDisabled()
        {
            _eventManager.EntityAdded -= Events_EntityAdded;
            _eventManager.EntityMerged -= Events_EntityMerged;
        }

        private void Events_EntityAdded(object sender, EntityAddedEventArgs e)
        {
            string userName = _sessionInfoProvider.GetCurrentUser();
            DateTime dateTime = _sessionInfoProvider.GetCurrentDateTime();

            string annotationKey = typeof(AuditingPluginMetadata).FullName;
            var annotation = e.EntityEntry.Metadata.FindAnnotation(annotationKey);
            if (annotation != null)
            {
                AuditingPluginMetadata props = (AuditingPluginMetadata)annotation.Value;

                if (props.CreatedBy != null)
                    e.EntityEntry.Property(props.CreatedBy.Name).CurrentValue = userName;

                if (props.CreatedDate != null)
                    e.EntityEntry.Property(props.CreatedDate.Name).CurrentValue = props.GetValueForDate(props.CreatedDate.ClrType, dateTime);
            }
        }

        private void Events_EntityMerged(object sender, EntityMergedEventArgs e)
        {
            if (e.Modified)
            {
                string userName = _sessionInfoProvider.GetCurrentUser();
                DateTime dateTime = _sessionInfoProvider.GetCurrentDateTime();

                string annotationKey = typeof(AuditingPluginMetadata).FullName;
                var annotation = e.EntityEntry.Metadata.FindAnnotation(annotationKey);
                if (annotation != null)
                {
                    AuditingPluginMetadata props = (AuditingPluginMetadata)annotation.Value;

                    if (props.ModifiedBy != null)
                        e.EntityEntry.Property(props.ModifiedBy.Name).CurrentValue = userName;

                    if (props.ModifiedDate != null)
                        e.EntityEntry.Property(props.ModifiedDate.Name).CurrentValue = props.GetValueForDate(props.ModifiedDate.ClrType, dateTime);
                }
            }
        }
    }
}