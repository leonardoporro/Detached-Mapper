using Detached.Mappers.EntityFramework.Context;
using Detached.Mappers.EntityFramework.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public static class DbContextExtensions
    {
        public static IQueryable<TProjection> Project<TEntity, TProjection>(this DbContext dbContext, IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return dbContext.GetService<DetachedQueryProvider>().Project<TProjection, TEntity>(query);
        }

        public static Task<TEntity> MapAsync<TEntity>(this DbContext dbContext, object entityOrDTO, MapperParameters parameters = null)
            where TEntity : class
        {
            Task<TEntity> task = Task.Run(() => Map<TEntity>(dbContext, entityOrDTO, parameters));
            task.ConfigureAwait(false);
            return task;
        }

        public static TEntity Map<TEntity>(this DbContext dbContext, object entityOrDTO, MapperParameters parameters = null)
            where TEntity : class
        {
            Mapper mapper = dbContext.GetService<Mapper>();
            DetachedQueryProvider queryProvider = dbContext.GetService<DetachedQueryProvider>();

            if (parameters == null)
                parameters = new MapperParameters();

            var context = new EntityFrameworkMapperContext(dbContext, queryProvider, parameters);

            return (TEntity)dbContext.GetService<Mapper>().Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), context);
        }

        public static async Task ImportJsonAsync<TEntity>(this DbContext dbContext, Stream stream)
            where TEntity : class
        {
            JsonSerializerOptions jsonSerializerOptions = dbContext.GetService<JsonSerializerOptions>();

            foreach (TEntity entity in await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(stream, jsonSerializerOptions))
            {
                if (entity != null)
                {
                    var result = await MapAsync<TEntity>(dbContext, entity, new MapperParameters { EnsureAggregations = true });
                }
            }
        }

        public static async Task ImportJsonAsync<TEntity>(this DbContext dbContext, string json)
            where TEntity : class
        {
            JsonSerializerOptions jsonSerializerOptions = dbContext.GetService<JsonSerializerOptions>();

            foreach (TEntity entity in JsonSerializer.Deserialize<IEnumerable<TEntity>>(json, jsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, entity, new MapperParameters { EnsureAggregations = true });
                }
            }
        }
    }
}