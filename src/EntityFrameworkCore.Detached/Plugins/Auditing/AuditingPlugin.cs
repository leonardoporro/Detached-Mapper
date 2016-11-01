using EntityFrameworkCore.Detached.Events;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Auditing
{
    public class AuditingPlugin : IDetachedPlugin
    {
        IDetachedContext _detachedContext;
        ISessionInfoProvider _sessionInfoProvider;

        public AuditingPlugin(ISessionInfoProvider sessionInfoProvider)
        {
            _sessionInfoProvider = sessionInfoProvider;
        }

        public bool IsEnabled { get; set; } = true;

        public int Priority { get; } = 0;

        public void Initialize(IDetachedContext detachedContext)
        {
            _detachedContext = detachedContext;
            _detachedContext.Events.EntityAdded += Events_EntityAdded;
            _detachedContext.Events.EntityMerged += Events_EntityMerged;
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

        public void Dispose()
        {
            _detachedContext.Events.EntityAdded -= Events_EntityAdded;
            _detachedContext.Events.EntityMerged -= Events_EntityMerged;
        }
    }
}