using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Conventions
{
    public interface ICustomConventionBuilder
    {
        void AddConventions(ConventionSet conventionSet);

        int Priority { get; }
    }
}
