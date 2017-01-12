using Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace Detached.EntityFramework.Plugins.Auditing.Conventions
{
    public class ModifiedByPropertyAttributeConvention : PropertyAttributeConvention<ModifiedByAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, ModifiedByAttribute attribute, MemberInfo clrMember)
        {
            var auditProps = AuditingPluginMetadata.GetMetadata(propertyBuilder.Metadata.DeclaringEntityType);
            auditProps.ModifiedBy = propertyBuilder.Metadata;
            propertyBuilder.Metadata.DetachedIgnore();
            return propertyBuilder;
        }
    }
}
