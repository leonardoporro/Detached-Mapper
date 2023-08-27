using Detached.Mappers.EntityFramework.Queries;
using Detached.PatchTypes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapper : Mapper
    {
        public EntityMapper(MapperOptions mapperOptions, Dictionary<string, string> concurrencyTokens)
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
            ConcurrencyTokens = concurrencyTokens;
        }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public Dictionary<string, string> ConcurrencyTokens { get; }

        public QueryProvider QueryProvider { get; } 

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

            var context = new EntityMapContext(this, dbContext, QueryProvider, parameters);

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