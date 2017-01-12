using Detached.EntityFramework.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Detached.EntityFramework.Plugins.PartialKey
{
    public class PartialKeyConventions : ICustomConventionBuilder
    {
        public int Priority { get; } = 0;

        public void AddConventions(ConventionSet conventionSet)
        {
            conventionSet.EntityTypeAddedConventions.Add(new PartialKeyEntityConvention());
        }
    }
}
