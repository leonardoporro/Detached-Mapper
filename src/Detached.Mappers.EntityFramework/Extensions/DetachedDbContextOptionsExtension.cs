using Detached.Mappers.EntityFramework.Conventions;
using Detached.Mappers.EntityFramework.Queries;
using Detached.Mappers.Model;
using Detached.Mappers.Patching;
using Detached.Mappers.TypeMaps;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework.Extensions
{
    public class DetachedDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public DetachedDbContextOptionsExtension(MapperOptions mapperOptions)
        {
            Info = new DetachedDbContextOptionsExtensionInfo(this);
            MapperOptions = mapperOptions;
        }

        public DbContextOptionsExtensionInfo Info { get; }

        public MapperOptions MapperOptions { get; }

        public bool IsConfigured { get; private set; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddScoped(sp =>
            {
                if (!IsConfigured)
                {
                    ICurrentDbContext currentDbContext = sp.GetRequiredService<ICurrentDbContext>();
                    MapperOptions.Conventions.Add(new IsEntityConvention(currentDbContext.Context.Model));

                    foreach(IMapperCustomizer customizer in  sp.GetServices<IMapperCustomizer>())
                    {
                        customizer.Customize(currentDbContext.Context, MapperOptions);
                    }

                    MethodInfo configureMapperMethodInfo = currentDbContext.Context.GetType().GetMethod("OnMapperCreating");
                    if (configureMapperMethodInfo != null)
                    {
                        var parameters = configureMapperMethodInfo.GetParameters();
                        if (parameters.Length != 1 && parameters[0].ParameterType != typeof(MapperOptions))
                        {
                            throw new ArgumentException($"ConfigureMapper method must have a single argument of type MapperOptions");
                        }

                        configureMapperMethodInfo.Invoke(currentDbContext.Context, new[] { MapperOptions });
                    }

                    IsConfigured = true;
                }

                return MapperOptions;
            });

            services.AddScoped<PatchJsonConverterFactory>();
            services.AddScoped<TypeMapFactory>();
            services.AddScoped(sp =>
            {
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.AllowTrailingCommas = true;
                jsonSerializerOptions.IgnoreReadOnlyProperties = true;
                jsonSerializerOptions.IgnoreReadOnlyFields = true;
                jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                jsonSerializerOptions.PropertyNameCaseInsensitive = false;
                jsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                jsonSerializerOptions.Converters.Add(sp.GetRequiredService<PatchJsonConverterFactory>());
                return jsonSerializerOptions;
            });
            services.AddScoped<Mapper>();
            services.AddScoped<DetachedQueryProvider>();
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}