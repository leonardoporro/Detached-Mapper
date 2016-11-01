using EntityFrameworkCore.Detached.Conventions;
using EntityFrameworkCore.Detached.Plugins.Auditing.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Detached.Plugins.Auditing
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