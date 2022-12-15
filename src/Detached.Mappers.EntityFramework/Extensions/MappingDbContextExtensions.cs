using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public static class MappingDbContextExtensions
    {
        public static EFMapper GetMapper(this DbContext dbContext)
        {
            return dbContext.GetMapper(null);
        }

        public static IQueryable<TProjection> Project<TEntity, TProjection>(this DbContext dbContext, IQueryable<TEntity> query, MapParameters mapParams = null)
            where TEntity : class
            where TProjection : class
        {
            return dbContext.GetMapper().Project<TEntity, TProjection>(query);
        }

        public static Task<TEntity> MapAsync<TEntity>(this DbContext dbContext, object entityOrDTO, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper().MapAsync<TEntity>(dbContext, entityOrDTO, mapParams);
        }

        public static TEntity Map<TEntity>(this DbContext dbContext, object entityOrDTO, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper().Map<TEntity>(dbContext, entityOrDTO, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper().MapJsonAsync<TEntity>(dbContext, stream, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper().MapJsonAsync<TEntity>(dbContext, json, mapParams);
        }

        public static Task MapJsonFileAsync<TEntity>(this DbContext dbContext, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetMapper().MapJsonFileAsync<TEntity>(dbContext, filePath, mapParams);
        }

        public static Task MapJsonResourceAsync<TEntity>(this DbContext dbContext, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetMapper().MapJsonResourceAsync<TEntity>(dbContext, resourceName, assembly, mapParams);
        }
    }
}