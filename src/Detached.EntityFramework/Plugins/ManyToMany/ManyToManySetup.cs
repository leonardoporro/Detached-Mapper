using Detached.EntityFramework.Plugins.ManyToMany.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Plugins.ManyToMany
{
    public static class ManyToManySetup
    {
        public static IServiceCollection AddDetachedManyToManyHelper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDetachedPlugin, ManyToManyPlugin>();
            serviceCollection.AddConventionBuilder<ManyToManyConventions>();
            return serviceCollection;
        }

        public static DetachedOptionsExtension UseManyToManyHelper(this DetachedOptionsExtension detachedOptions)
        {
            detachedOptions.DetachedServices.AddDetachedManyToManyHelper();
            return detachedOptions;
        }
    }
}
