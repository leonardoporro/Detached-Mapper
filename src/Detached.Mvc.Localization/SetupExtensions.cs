using Detached.Mvc.Localization.DataAnnotations;
using Detached.Mvc.Localization.Json;
using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace Detached.Mvc.Localization
{
    public static class SetupExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services, Action<JsonLocalizationOptions> configure = null)
        {
            JsonLocalizationOptions options = new JsonLocalizationOptions();

            services.TryAddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IStringLocalizerFactory>(s => new JsonStringLocalizerFactory(s.GetRequiredService<IFileSystem>(), options));

            return services;
        }

        public static IResourceMapperBuilder AddResourceMapper(this IServiceCollection serviceCollection, Action<ResourceMapperOptions> configure)
        {
            ResourceMapperOptions options = new ResourceMapperOptions();
            configure?.Invoke(options);
            serviceCollection.AddSingleton<IResourceMapper>(s => new ResourceMapper(options));

            return new ResourceMapperBuilder { Services = serviceCollection };
        }

        public static IResourceMapperBuilder AddAutomaticDisplayMetadataLocalization(this IResourceMapperBuilder mapperBuilder)
        {
            mapperBuilder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, DetachedDisplayMetadataConfigureOptions>());
            return mapperBuilder;
        }

        public static IResourceMapperBuilder AddAutomaticValidationAttributeLocalization(this IResourceMapperBuilder mapperBuilder)
        {
            mapperBuilder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, DetachedModelValidatorConfigureOptions>());      
            return mapperBuilder;
        }
    }
}