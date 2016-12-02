using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Conventions
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
