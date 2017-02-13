using Detached.Mvc.Localization.DataAnnotations;
using Detached.Mvc.Localization.Errors;
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
        /// <summary>
        /// Adds localization (similar to .AddLocalization()) based on .json files.
        /// Use configuration to define a folder for the .json files if needed.
        /// The localizer will look for files specified by 'baseName' and 'location' parameters of the 
        /// IStringLocalizerFactory.Create method.
        /// </summary>
        /// <param name="services">The service collection to extend.</param>
        /// <param name="configure">Localization configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services, Action<JsonLocalizationOptions> configure = null)
        {
            JsonLocalizationOptions options = new JsonLocalizationOptions();

            services.TryAddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IStringLocalizerFactory>(s => new JsonStringLocalizerFactory(s.GetRequiredService<IFileSystem>(), options));

            return services;
        }

        /// <summary>
        /// Adds a ResouceMapper.
        /// ResourceMapper helps translating unique Clr Type name and namespaces to the format that the current
        /// IStringLocalizerFactory needs.
        /// </summary>
        /// <param name="serviceCollection">The service collection to extend.</param>
        /// <param name="configure">Action that sets the configuration options.</param>
        public static IResourceMapperBuilder AddResourceMapper(this IServiceCollection serviceCollection, Action<ResourceMapperOptions> configure)
        {
            ResourceMapperOptions options = new ResourceMapperOptions();
            configure?.Invoke(options);
            serviceCollection.AddSingleton<IResourceMapper>(s => new ResourceMapper(options));
            serviceCollection.AddSingleton<IErrorLocalizer, ErrorLocalizer>();

            return new ResourceMapperBuilder { Services = serviceCollection };
        }

        /// <summary>
        /// Localizes all the display names of the model properties by using the resource mapper to generate the keys.
        /// Display names are later used in validation and error messages.
        /// </summary>
        /// <param name="mapperBuilder">The builder to extend.</param>
        public static IResourceMapperBuilder AddAutomaticDisplayMetadataLocalization(this IResourceMapperBuilder mapperBuilder)
        {
            mapperBuilder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, DetachedDisplayMetadataConfigureOptions>());
            return mapperBuilder;
        }

        /// <summary>
        /// Localizes the validation messages of the validators added using DataAttributes by using the resource mapper to
        /// generate the keys.
        /// </summary>
        /// <param name="mapperBuilder">The builder to extend.</param>
        public static IResourceMapperBuilder AddAutomaticValidationAttributeLocalization(this IResourceMapperBuilder mapperBuilder)
        {
            mapperBuilder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, DetachedModelValidatorConfigureOptions>());      
            return mapperBuilder;
        }
    }
}