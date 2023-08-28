using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public static class MappingDbContextExtensions
    {
        public static IQueryable<TProjection> Project<TEntity, TProjection>(this DbContext dbContext, IQueryable<TEntity> query, MapParameters mapParams = null)
            where TEntity : class
            where TProjection : class
        {
            return dbContext.GetEntityMapper().Project<TEntity, TProjection>(query, ProfileKey.Empty);
        }

        public static Task<TEntity> MapAsync<TEntity>(this DbContext dbContext, object entityOrDTO, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().MapAsync<TEntity>(dbContext, ProfileKey.Empty, entityOrDTO, mapParams);
        }

        public static TEntity Map<TEntity>(this DbContext dbContext, object entityOrDTO, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().Map<TEntity>(dbContext, ProfileKey.Empty, entityOrDTO, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonAsync<TEntity>(dbContext, ProfileKey.Empty, stream, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonAsync<TEntity>(dbContext, ProfileKey.Empty, json, mapParams);
        }

        public static Task MapJsonFileAsync<TEntity>(this DbContext dbContext, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonFileAsync<TEntity>(dbContext, ProfileKey.Empty, filePath, mapParams);
        }

        public static Task MapJsonResourceAsync<TEntity>(this DbContext dbContext, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonResourceAsync<TEntity>(dbContext, ProfileKey.Empty, resourceName, assembly, mapParams);
        }
    }
}