using Detached.Mapping;
using Detached.Model;
using Detached.Patching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Detached
{
    public static class Package
    {
        public static void AddDetached(this IServiceCollection services)
        {
            services.AddOptions<MapperModelOptions>();
            services.TryAddSingleton<Mapper>();
            services.TryAddSingleton<TypeMapFactory>();
            services.TryAddSingleton<PatchJsonConverterFactory>();
        }
    }
}