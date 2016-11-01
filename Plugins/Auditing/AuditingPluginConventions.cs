using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.Detached.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using EntityFrameworkCore.Detached.Plugins.Auditing.Conventions;

namespace EntityFrameworkCore.Detached.Plugins.Auditing
{
    public class AuditingPluginConventions : ICustomConventionBuilder
    {
        public int Priority { get; } = 0;

        public void AddConventions(ConventionSet conventionSet)
        {
            conventionSet.PropertyAddedConventions.Add(new CreatedByPropertyAttributeConvention());
            conventionSet.PropertyAddedConventions.Add(new CreatedDatePropertyAttributeConvention());
            conventionSet.PropertyAddedConventions.Add(new ModifiedByPropertyAttributeConvention());
            conventionSet.PropertyAddedConventions.Add(new ModifiedDatePropertyAttributeConvention());
        }
    }
}
