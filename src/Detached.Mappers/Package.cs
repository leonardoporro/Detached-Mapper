using Detached.PatchTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Detached.Mappers
{
    public static class Package
    {
        public static void AddDetached(this IServiceCollection services)
        {
            services.AddOptions<MapperOptions>();
            services.TryAddSingleton<Mapper>(); 
            services.TryAddSingleton(sp => new PatchJsonConverterFactory(sp.GetRequiredService<Mapper>()));
        }
    }
}