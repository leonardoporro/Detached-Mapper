using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Plugins.Auditing.Conventions
{
    public class CreatedDatePropertyAttributeConvention : PropertyAttributeConvention<CreatedDateAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, CreatedDateAttribute attribute, MemberInfo clrMember)
        {
            var auditProps = AuditingPluginMetadata.GetMetadata(propertyBuilder.Metadata.DeclaringEntityType);
            auditProps.CreatedDate = propertyBuilder.Metadata;
            propertyBuilder.Metadata.Ignore();
            return propertyBuilder;
        }
    }
}
