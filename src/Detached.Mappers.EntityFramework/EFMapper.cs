using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Conventions;
using Detached.Mappers.EntityFramework.Queries;
using Detached.PatchTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public class EFMapper : Mapper
    {
        public EFMapper(DbContext dbContext, MapperOptions mapperOptions)
            : base(mapperOptions)
        {
            JsonSerializerOptions = new JsonSerializerOptions();
            JsonSerializerOptions.AllowTrailingCommas = true;
            JsonSerializerOptions.IgnoreReadOnlyProperties = true;
            JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            JsonSerializerOptions.Converters.Add(new PatchJsonConverterFactory());

            QueryProvider = new QueryProvider(this); 

            Options.TypeConventions.Add(new EFConventions(dbContext.Model));

            foreach (IMapperCustomizer customizer in dbContext.GetInfrastructure().GetServices<IMapperCustomizer>())
            {
                customizer.Customize(dbContext, mapperOptions);
            }

            MethodInfo configureMapperMethodInfo = dbContext.GetType().GetMethod("OnMapperCreating");
            if (configureMapperMethodInfo != null)
            {
                var parameters = configureMapperMethodInfo.GetParameters();
                if (parameters.Length != 1 && parameters[0].ParameterType != typeof(MapperOptions))
                {
                    throw new ArgumentException($"ConfigureMapper method must have a single argument of type MapperOptions");
                }

                configureMapperMethodInfo.Invoke(dbContext, new[] { mapperOptions });
            }

            ContextModel = new EFMapContextModel();

            foreach (IEntityType entityType in dbContext.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.IsConcurrencyToken)
                    {
                        ContextModel.ConcurrencyTokens.Add(entityType.Name, property.Name);
                    }
                }
            }
        }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public QueryProvider QueryProvider { get; }

        public EFMapContextModel ContextModel { get; }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
           where TEntity : class
           where TProjection : class
        {
            return QueryProvider.Project<TEntity, TProjection>(query);
        }

        public Task<TEntity> MapAsync<TEntity>(DbContext dbContext, object entityOrDTO, MapParameters parameters = null)
            where TEntity : class
        {
            Task<TEntity> task = Task.Run(() => Map<TEntity>(dbContext, entityOrDTO, parameters));
            task.ConfigureAwait(false);
            return task;
        }

        public TEntity Map<TEntity>(DbContext dbContext, object entityOrDTO, MapParameters parameters = null)
            where TEntity : class
        {
            if (parameters == null)
            {
                parameters = new MapParameters();
            }

            var context = new EFMapContext(dbContext, QueryProvider, ContextModel, parameters);

            return (TEntity)Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), context);
        }

        public async Task MapJsonAsync<TEntity>(DbContext dbContext, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            if (mapParams == null)
            {
                mapParams = new MapParameters { AddAggregations = true };
            }

            foreach (TEntity entity in await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(stream, JsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, entity, mapParams);
                }
            }
        }

        public async Task MapJsonAsync<TEntity>(DbContext dbContext, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            if (mapParams == null)
            {
                mapParams = new MapParameters { AddAggregations = true };
            }

            foreach (TEntity entity in JsonSerializer.Deserialize<IEnumerable<TEntity>>(json, JsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, entity, mapParams);
                }
            }
        }

        public async Task MapJsonFileAsync<TEntity>(DbContext dbContext, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            using (Stream fileStream = File.OpenRead(filePath))
            {
                await MapJsonAsync<TEntity>(dbContext, fileStream, mapParams);
            }
        }

        public async Task MapJsonResourceAsync<TEntity>(DbContext dbContext, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            using (Stream fileStream = assembly.GetManifestResourceStream(resourceName))
            {
                await MapJsonAsync<TEntity>(dbContext, fileStream, mapParams);
            }
        }
    }
}