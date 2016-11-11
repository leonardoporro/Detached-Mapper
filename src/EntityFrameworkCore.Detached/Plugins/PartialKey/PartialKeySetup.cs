using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.PartialKey
{
    public static class PartialKeySetup
    {
        public static IServiceCollection AddDetachedPartialKey(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddConventionBuilder<PartialKeyConventions>();
            return serviceCollection;
        }

        public static DetachedOptionsExtension UsePartialKey(this DetachedOptionsExtension detachedOptions)
        {
            detachedOptions.DetachedServices.AddDetachedPartialKey();
            return detachedOptions;
        }
    }
}
