using EntityFrameworkCore.Detached.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace EntityFrameworkCore.Detached
{
    /// <summary>
    /// Provides extension methods to hande audit information metadata.
    /// </summary>
    public static class AuditExtensions
    {
        const string DETACHED_PROPERTY_IS_AUDIT = "DETACHED_PROPERTY_IS_AUDIT";

        /// <summary>
        /// Gets or creates the metadata needed to set audit information.
        /// </summary>
        /// <param name="entityType">Metadata of the entity to be audited.</param>
        /// <returns>An instance of AuditProperties.</returns>
        public static AuditProperties GetOrCreateAuditProperties(this EntityType entityType)
        {
            AuditProperties auditInfo;
            string annotationKey = typeof(AuditProperties).FullName;
            var annotation = entityType.FindAnnotation(annotationKey);
            if (annotation == null)
            {
                auditInfo = new AuditProperties();
                entityType.SetAnnotation(annotationKey, auditInfo, ConfigurationSource.Explicit);
            }
            else
            {
                auditInfo = (AuditProperties)annotation.Value;
            }
            return auditInfo;
        }

        /// <summary>
        /// Sets audit info for a recently created entity.
        /// </summary>
        /// <param name="entityType">Metadata of the entity being audited.</param>
        /// <param name="instance">Instance of the entity being audited.</param>
        /// <param name="user">Current user.</param>
        /// <param name="dateTime">Current DateTime.</param>
        public static void SetCreatedAuditInfo(this EntityEntry entry, string user, DateTime dateTime)
        {
            string annotationKey = typeof(AuditProperties).FullName;
            var annotation = entry.Metadata.FindAnnotation(annotationKey);
            if (annotation != null)
            {
                AuditProperties props = (AuditProperties)annotation.Value;

                if (props.CreatedBy != null)
                   entry.Property(props.CreatedBy.Name).CurrentValue = user;

                if (props.CreatedDate != null)
                    entry.Property(props.CreatedDate.Name).CurrentValue = props.GetValueForDate(props.CreatedDate.ClrType, dateTime); ;
            }
        }

        /// <summary>
        /// Sets audit info for a recently created entity.
        /// </summary>
        /// <param name="entityType">Metadata of the entity being audited.</param>
        /// <param name="instance">Instance of the entity being audited.</param>
        /// <param name="user">Current user.</param>
        /// <param name="dateTime">Current DateTime.</param>
        public static void SetModifiedAuditInfo(this EntityEntry entry, string user, DateTime dateTime)
        {
            string annotationKey = typeof(AuditProperties).FullName;
            var annotation = entry.Metadata.FindAnnotation(annotationKey);
            if (annotation != null)
            {
                AuditProperties props = (AuditProperties)annotation.Value;

                if (props.ModifiedBy != null)
                    entry.Property(props.ModifiedBy.Name).CurrentValue = user;

                if (props.ModifiedDate != null)
                    entry.Property(props.ModifiedDate.Name).CurrentValue = props.GetValueForDate(props.ModifiedDate.ClrType, dateTime);
            }
        }
    }
}
