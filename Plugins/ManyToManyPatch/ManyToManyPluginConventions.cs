using EntityFrameworkCore.Detached.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EntityFrameworkCore.Detached.Plugins.ManyToManyPatch
{
    public class ManyToManyPluginConventions : ICustomConventionBuilder
    {
        public int Priority { get; } = 0;

        public void AddConventions(ConventionSet conventionSet)
        {
            conventionSet.NavigationAddedConventions.Add(new ManyToManyPluginAttributeConvention());
        }
    }
}
