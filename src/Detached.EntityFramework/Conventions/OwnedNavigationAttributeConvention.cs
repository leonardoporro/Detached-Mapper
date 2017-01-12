using Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Detached.EntityFramework.Conventions
{
    /// <summary>
    /// Handles [Owned] attribute.
    /// </summary>
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
