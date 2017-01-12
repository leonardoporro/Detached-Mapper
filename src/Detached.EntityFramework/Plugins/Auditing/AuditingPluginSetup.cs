using Detached.EntityFramework.Conventions;
using Detached.EntityFramework.Plugins.Auditing.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.EntityFramework.Plugins.Auditing
{
    public static class AuditingPluginSetup
    {
        public static IServiceCollection AddDetachedAuditing(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDetachedPlugin, AuditingPlugin>();
            serviceCollection.AddScoped<ICustomConventionBuilder, AuditingPluginConventions>();

            return serviceCollection;
        }

        public static DetachedOptionsExtension UseAuditing(this DetachedOptionsExtension detachedOptions)
        {
            detachedOptions.DetachedServices.AddDetachedAuditing();
            return detachedOptions;
        }
    }
}