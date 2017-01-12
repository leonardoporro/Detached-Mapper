using Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Conventions
{
    /// <summary>
    /// Handles [Associated] attribute.
    /// </summary>
    public class AssociatedNavigationAttributeConvention : NavigationAttributeNavigationConvention<AssociatedAttribute>
    {
        public override InternalRelationshipBuilder Apply(InternalRelationshipBuilder relationshipBuilder, Navigation navigation, AssociatedAttribute attribute)
        {
            navigation.SetAnnotation(typeof(AssociatedAttribute).FullName, attribute, ConfigurationSource.DataAnnotation);
            relationshipBuilder.DeleteBehavior(DeleteBehavior.SetNull, ConfigurationSource.DataAnnotation);

            return relationshipBuilder;
        }
    }
}
