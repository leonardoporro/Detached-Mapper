using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Conventions
{
    public class OwnedNavigationAttributeConvention : NavigationAttributeNavigationConvention<OwnedAttribute>
    {
        public override InternalRelationshipBuilder Apply(InternalRelationshipBuilder relationshipBuilder, Navigation navigation, OwnedAttribute attribute)
        {
            navigation.SetAnnotation(typeof(OwnedAttribute).FullName, attribute, ConfigurationSource.DataAnnotation);
            relationshipBuilder.DeleteBehavior(DeleteBehavior.Cascade, ConfigurationSource.DataAnnotation);
            return relationshipBuilder;
        }
    }
}
