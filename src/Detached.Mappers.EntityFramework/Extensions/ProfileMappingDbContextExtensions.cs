using Detached.Mappers.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public static class ProfileMappingDbContextExtensions
    {
        public static EFMapper GetMapper(this DbContext dbContext, object profileKey)
        {
            EFMapperProfiles services = dbContext.GetService<EFMapperProfiles>();
            if (services == null)
            {
                throw new MapperException($"Detached is not configured. Did you miss UseMapping() call?");
            }

            return services.GetInstance(profileKey, dbContext);
        }

        public static IQueryable<TProjection> Project<TEntity, TProjection>(this DbContext dbContext, object profileKey, IQueryable<TEntity> query, MapParameters mapParams = null)
            where TEntity : class
            where TProjection : class
        {
            return dbContext.GetMapper(profileKey).Project<TEntity, TProjection>(query);
        }

        public static Task<TEntity> MapAsync<TEntity>(this DbContext dbContext, object profileKey, object entityOrDTO, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper(profileKey).MapAsync<TEntity>(dbContext, entityOrDTO, mapParams);
        }

        public static TEntity Map<TEntity>(this DbContext dbContext, object profileKey, object entityOrDTO, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper(profileKey).Map<TEntity>(dbContext, entityOrDTO, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, object profileKey, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper(profileKey).MapJsonAsync<TEntity>(dbContext, stream, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, object profileKey, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetMapper(profileKey).MapJsonAsync<TEntity>(dbContext, json, mapParams);
        }

        public static Task MapJsonFileAsync<TEntity>(this DbContext dbContext, object profileKey, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetMapper(profileKey).MapJsonFileAsync<TEntity>(dbContext, filePath, mapParams);
        }

        public static Task MapJsonResourceAsync<TEntity>(this DbContext dbContext, object profileKey, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetMapper(profileKey).MapJsonResourceAsync<TEntity>(dbContext, resourceName, assembly, mapParams);
        }
    }
}