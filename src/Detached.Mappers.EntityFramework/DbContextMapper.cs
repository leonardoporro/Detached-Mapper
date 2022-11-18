using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Conventions;
using Detached.Mappers.EntityFramework.Queries;
using Detached.PatchTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework
{
    public class DbContextMapper : Mapper
    {
        public DbContextMapper(DbContext dbContext, MapperOptions options)
            : base(options)
        {
            MapperOptions = options;

            JsonSerializerOptions = new JsonSerializerOptions();
            JsonSerializerOptions.AllowTrailingCommas = true;
            JsonSerializerOptions.IgnoreReadOnlyProperties = true;
            JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            JsonSerializerOptions.Converters.Add(new PatchJsonConverterFactory());

            QueryProvider = new DetachedQueryProvider(options);

            MapperOptions.TypeConventions.Add(new EntityFrameworkConvention(dbContext.Model));

            foreach (IMapperCustomizer customizer in dbContext.GetInfrastructure().GetServices<IMapperCustomizer>())
            {
                customizer.Customize(dbContext, MapperOptions);
            }

            MethodInfo configureMapperMethodInfo = dbContext.GetType().GetMethod("OnMapperCreating");
            if (configureMapperMethodInfo != null)
            {
                var parameters = configureMapperMethodInfo.GetParameters();
                if (parameters.Length != 1 && parameters[0].ParameterType != typeof(MapperOptions))
                {
                    throw new ArgumentException($"ConfigureMapper method must have a single argument of type MapperOptions");
                }

                configureMapperMethodInfo.Invoke(dbContext, new[] { MapperOptions });
            } 
        }

        public Mapper Mapper { get; }

        public MapperOptions MapperOptions { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public DetachedQueryProvider QueryProvider { get; }
    }
}