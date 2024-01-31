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
        public static EntityMapper GetEntityMapper(this DbContext dbContext, object profileKey = null)
        {
            var factory = dbContext.GetService<EntityMapperFactory>();
            if (factory == null)
            {
                throw new MapperException($"Detached is not configured. Did you miss UseMapping() call?");
            }

            return factory.CreateMapper(dbContext, profileKey);
        }

        public static Mapper GetMapper(this DbContext dbContext, object profileKey = null)
        {
            return dbContext.GetEntityMapper(profileKey).Mapper;
        }

        public static IQueryable<TProjection> Project<TEntity, TProjection>(this DbContext dbContext, object profileKey, IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return dbContext.GetEntityMapper(profileKey).Project<TEntity, TProjection>(query);
        }

        public static Task<TEntity> MapAsync<TEntity>(this DbContext dbContext, object profileKey, object entityOrDto, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper(profileKey).MapAsync<TEntity>(dbContext, entityOrDto, mapParams);
        }

        public static TEntity Map<TEntity>(this DbContext dbContext, object profileKey, object entityOrDto, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper(profileKey).Map<TEntity>(dbContext, entityOrDto, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, object profileKey, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper(profileKey).MapJsonAsync<TEntity>(dbContext, stream, mapParams);
        }

        public static Task MapJsonAsync<TEntity>(this DbContext dbContext, object profileKey, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            return dbContext.GetEntityMapper(profileKey).MapJsonAsync<TEntity>(dbContext, json, mapParams);
        }

        public static Task MapJsonFileAsync<TEntity>(this DbContext dbContext, object profileKey, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetEntityMapper(profileKey).MapJsonFileAsync<TEntity>(dbContext, filePath, mapParams);
        }

        public static Task MapJsonResourceAsync<TEntity>(this DbContext dbContext, object profileKey, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            return dbContext.GetEntityMapper(profileKey).MapJsonResourceAsync<TEntity>(dbContext, resourceName, assembly, mapParams);
        }
    }
}