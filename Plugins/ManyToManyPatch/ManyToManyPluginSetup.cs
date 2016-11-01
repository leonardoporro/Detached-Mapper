using EntityFrameworkCore.Detached.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Detached.Plugins.ManyToManyPatch
{
    public static class ManyToManyPluginSetup
    {
        public static IServiceCollection AddDetachedManyToManyPatch(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICustomConventionBuilder, ManyToManyPluginConventions>();
            serviceCollection.AddScoped<IDetachedPlugin, ManyToManyPlugin>();
            return serviceCollection;
        }

        public static DetachedOptionsExtensions UseManyToManyPatch(this DetachedOptionsExtensions detachedOptions)
        {
            detachedOptions.ServiceCollection.AddDetachedManyToManyPatch();
            detachedOptions.AddPluginType<ManyToManyPlugin>();
            return detachedOptions;
        }
    }
}
