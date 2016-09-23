using EntityFrameworkCore.Detached.DataAnnotations;
using EntityFrameworkCore.Detached.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Conventions
{
    public class CreatedDatePropertyAttributeConvention : PropertyAttributeConvention<CreatedDateAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, CreatedDateAttribute attribute, MemberInfo clrMember)
        {
            AuditProperties auditProps = propertyBuilder.Metadata.DeclaringEntityType.GetOrCreateAuditProperties();
            auditProps.CreatedDate = propertyBuilder.Metadata;
            propertyBuilder.Metadata.DetachedIgnore();
            return propertyBuilder;
        }
    }
}
