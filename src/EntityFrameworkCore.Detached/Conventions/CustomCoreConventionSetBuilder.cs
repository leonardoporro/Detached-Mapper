using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkCore.Detached.Conventions
{
    public class CustomCoreConventionSetBuilder : CoreConventionSetBuilder
    {
        IEnumerable<ICustomConventionBuilder> _conventionBuilders;

        public CustomCoreConventionSetBuilder(IServiceProvider serviceProvider)
        {
            _conventionBuilders = serviceProvider.GetServices<ICustomConventionBuilder>();
        }

        public override ConventionSet CreateConventionSet()
        {
            ConventionSet conventionSet = base.CreateConventionSet();
            conventionSet.NavigationAddedConventions.Add(new AssociatedNavigationAttributeConvention());
            conventionSet.NavigationAddedConventions.Add(new OwnedNavigationAttributeConvention());         
            foreach (ICustomConventionBuilder conventionBuilder in _conventionBuilders)
            {
                conventionBuilder.AddConventions(conventionSet);
            }
            return conventionSet;
        }
    }
}
