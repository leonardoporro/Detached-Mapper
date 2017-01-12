using Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace Detached.EntityFramework.Conventions
{
    public class IgnoredPropertyAttributeConvention : PropertyAttributeConvention<IgnoredAtrribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, IgnoredAtrribute attribute, MemberInfo clrMember)
        {
            propertyBuilder.Metadata.DetachedIgnore();
            return propertyBuilder;
        }
    }
}
