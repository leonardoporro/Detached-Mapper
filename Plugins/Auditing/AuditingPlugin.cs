using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Auditing
{
    public class AuditingPlugin : DetachedPlugin
    {
        const string DETACHED_PROPERTY_IS_AUDIT = "DETACHED_PROPERTY_IS_AUDIT";
        ISessionInfoProvider _sessionInfoProvider;

        public AuditingPlugin(ISessionInfoProvider sessionInfoProvider)
        {
            _sessionInfoProvider = sessionInfoProvider;
        }

        public override Task OnEntityAdded(EntityEntry newEntityEntry, INavigation navigation, EntityEntry parentEntityEntry)
        {
            string userName = _sessionInfoProvider.GetCurrentUser();
            DateTime dateTime = _sessionInfoProvider.GetCurrentDateTime();

            string annotationKey = typeof(AuditingPluginMetadata).FullName;
            var annotation = newEntityEntry.Metadata.FindAnnotation(annotationKey);
            if (annotation != null)
            {
                AuditingPluginMetadata props = (AuditingPluginMetadata)annotation.Value;

                if (props.CreatedBy != null)
                    newEntityEntry.Property(props.CreatedBy.Name).CurrentValue = userName;

                if (props.CreatedDate != null)
                    newEntityEntry.Property(props.CreatedDate.Name).CurrentValue = props.GetValueForDate(props.CreatedDate.ClrType, dateTime);
            }

            return Task.FromResult(0);
        }

        public override Task OnEntityMerged(EntityEntry persistedEntityEntry, object detachedEntity, INavigation navigation, bool modified, EntityEntry parentEntityEntry)
        {
            if (modified)
            {
                string userName = _sessionInfoProvider.GetCurrentUser();
                DateTime dateTime = _sessionInfoProvider.GetCurrentDateTime();

                string annotationKey = typeof(AuditingPluginMetadata).FullName;
                var annotation = persistedEntityEntry.Metadata.FindAnnotation(annotationKey);
                if (annotation != null)
                {
                    AuditingPluginMetadata props = (AuditingPluginMetadata)annotation.Value;

                    if (props.ModifiedBy != null)
                        persistedEntityEntry.Property(props.ModifiedBy.Name).CurrentValue = userName;

                    if (props.ModifiedDate != null)
                        persistedEntityEntry.Property(props.ModifiedDate.Name).CurrentValue = props.GetValueForDate(props.ModifiedDate.ClrType, dateTime);
                }
            }

            return Task.FromResult(0);
        }
    }
}