using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Conventions
{
    public class DetachedCoreConventionSetBuilder : CoreConventionSetBuilder
    {
        public override ConventionSet CreateConventionSet()
        {
            var set = base.CreateConventionSet();
            set.NavigationAddedConventions.Add(new AssociatedNavigationAttributeConvention());
            set.NavigationAddedConventions.Add(new OwnedNavigationAttributeConvention());

            return set;
        }
    }
}
