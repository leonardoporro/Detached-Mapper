using Detached.Mappers.EntityFramework.Profiles;
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
        public static EntityMapper GetEntityMapper(this DbContext dbContext)
        {
            var configuration = dbContext.GetService<EntityMapperFactory>();
            if (configuration == null)
            {
                throw new MapperException($"Detached is not configured. Did you miss UseMapping() call?");
            }

            return configuration.CreateMapper(dbContext);
        }

        public static Mapper GetMapper(this DbContext dbContext, ProfileKey profileKey)
        {
            return GetEntityMapper(dbContext).GetProfile(profileKey).Mapper;
        }

        public static IQueryable<TProjection> Project<TEntity, TProjection>(this DbContext dbContext, object profileKey, IQueryable<TEntity> query, MapParameters mapParams = null)
            where TEntity : class
            where TProjection : class
        {
            return dbContext.GetEntityMapper().Project<TEntity, TProjection>(query, new ProfileKey(profileKey));
        }

        public static Task<TEntity> MapAsync<TEntity>(this DbContext dbContext, object profileKey, object entityOrDto, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().MapAsync<TEntity>(dbContext, new ProfileKey(profileKey), entityOrDto, mapParams);
        }

        public static TEntity Map<TEntity>(this DbContext dbContext, object profileKey, object entityOrDto, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().Map<TEntity>(dbContext, new ProfileKey(profileKey), entityOrDto, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, object profileKey, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonAsync<TEntity>(dbContext, new ProfileKey(profileKey), stream, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, object profileKey, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonAsync<TEntity>(dbContext, new ProfileKey(profileKey), json, mapParams);
        }

        public static Task MapJsonFileAsync<TEntity>(this DbContext dbContext, object profileKey, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonFileAsync<TEntity>(dbContext, new ProfileKey(profileKey), filePath, mapParams);
        }

        public static Task MapJsonResourceAsync<TEntity>(this DbContext dbContext, object profileKey, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetEntityMapper().MapJsonResourceAsync<TEntity>(dbContext, new ProfileKey(profileKey), resourceName, assembly, mapParams);
        }
    }
}