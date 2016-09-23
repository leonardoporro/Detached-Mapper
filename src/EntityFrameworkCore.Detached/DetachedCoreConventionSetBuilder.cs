using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace EntityFrameworkCore.Detached.Conventions
{
    /// <summary>
    /// Custom ConventionSetBuilder that provides handling to [Associated] and [Owned] attributes.
    /// </summary>
    public class DetachedCoreConventionSetBuilder : CoreConventionSetBuilder
    {
        public override ConventionSet CreateConventionSet()
        {
            var set = base.CreateConventionSet(); 

            // detached
            set.NavigationAddedConventions.Add(new AssociatedNavigationAttributeConvention());
            set.NavigationAddedConventions.Add(new OwnedNavigationAttributeConvention());
            // many to many
            set.NavigationAddedConventions.Add(new ManyToManyPatchAttributeConvention());
            // audit
            set.PropertyAddedConventions.Add(new CreatedByPropertyAttributeConvention());
            set.PropertyAddedConventions.Add(new CreatedDatePropertyAttributeConvention());
            set.PropertyAddedConventions.Add(new ModifiedByPropertyAttributeConvention());
            set.PropertyAddedConventions.Add(new ModifiedDatePropertyAttributeConvention());

            return set;
        }
    }
}
