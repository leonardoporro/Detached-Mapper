using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            set.NavigationAddedConventions.Add(new AssociatedNavigationAttributeConvention());
            set.NavigationAddedConventions.Add(new OwnedNavigationAttributeConvention());
            set.NavigationAddedConventions.Add(new ManyToManyNavigationAttributeConvention());

            return set;
        }
    }
}
