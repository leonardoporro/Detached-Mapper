using EntityFrameworkCore.Detached.DataAnnotations;
using EntityFrameworkCore.Detached.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Conventions
{
    public class ModifiedDatePropertyAttributeConvention : PropertyAttributeConvention<ModifiedDateAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, ModifiedDateAttribute attribute, MemberInfo clrMember)
        {
            AuditProperties auditProps = propertyBuilder.Metadata.DeclaringEntityType.GetOrCreateAuditProperties();
            auditProps.ModifiedDate = propertyBuilder.Metadata;

            return propertyBuilder;
        }
    }
}
