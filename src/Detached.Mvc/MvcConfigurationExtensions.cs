using Detached.Mvc.Localization;
using Detached.Mvc.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Detached.Mvc
{
    public static class MvcConfigurationExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IMetadataProvider, MetadataProvider>();
            services.AddSingleton<IJsonStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizerFactory>(s => s.GetRequiredService<IJsonStringLocalizerFactory>());

            return services;
        }

        public static IServiceCollection AddLocalizationMetadata(this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, LocalizedMetadataProviderSetup>());

            return services;
        }
    }
}
