using Detached.Mappers;
using Detached.Mappers.Model;
using Detached.Mappers.Patching;
using Detached.Mappers.TypeMaps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Detached
{
    public static class Package
    {
        public static void AddDetached(this IServiceCollection services)
        {
            services.AddOptions<MapperOptions>();
            services.TryAddSingleton<Mapper>();
            services.TryAddSingleton<TypeMapFactory>();
            services.TryAddSingleton<PatchJsonConverterFactory>();
        }
    }
}