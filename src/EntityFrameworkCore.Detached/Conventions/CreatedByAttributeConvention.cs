using EntityFrameworkCore.Detached.DataAnnotations;
using EntityFrameworkCore.Detached.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Conventions
{
    public class CreatedByPropertyAttributeConvention : PropertyAttributeConvention<CreatedByAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, CreatedByAttribute attribute, MemberInfo clrMember)
        {
            AuditProperties auditProps = propertyBuilder.Metadata.DeclaringEntityType.GetOrCreateAuditProperties();
            auditProps.CreatedBy = propertyBuilder.Metadata;
            propertyBuilder.Metadata.DetachedIgnore();
            return propertyBuilder;
        }
    }
}
