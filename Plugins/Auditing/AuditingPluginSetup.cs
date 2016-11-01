using EntityFrameworkCore.Detached.Conventions;
using EntityFrameworkCore.Detached.Plugins.Auditing.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Auditing
{
    public static class AuditingPluginSetup
    {
        public static IServiceCollection AddDetachedAuditingPlugin(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDetachedPlugin, AuditingPlugin>();
            serviceCollection.AddScoped<ICustomConventionBuilder, AuditingPluginConventions>();

            return serviceCollection;
        }

        public static DetachedOptionsExtensions UseAuditingPlugin(this DetachedOptionsExtensions detachedOptions)
        {
            detachedOptions.ServiceCollection.AddDetachedAuditingPlugin();
            detachedOptions.AddPluginType<AuditingPlugin>();
            return detachedOptions;
        }
    }
}
