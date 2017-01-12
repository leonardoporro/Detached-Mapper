using Detached.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Detached.EntityFramework.Plugins.Auditing.Conventions
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
