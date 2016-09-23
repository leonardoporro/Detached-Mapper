using EntityFrameworkCore.Detached.DataAnnotations;
using EntityFrameworkCore.Detached.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Conventions
{
    public class ModifiedByPropertyAttributeConvention : PropertyAttributeConvention<ModifiedByAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, ModifiedByAttribute attribute, MemberInfo clrMember)
        {
            AuditProperties auditProps = propertyBuilder.Metadata.DeclaringEntityType.GetOrCreateAuditProperties();
            auditProps.ModifiedBy = propertyBuilder.Metadata;

            return propertyBuilder;
        }
    }
}
