using Detached.Mappers.TypeOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public static class DbContextExtensions
    {
        public static DbContextMapper GetMapper(this DbContext dbContext, object profileKey)
        {
            DbContextMapperCollection collection = dbContext.GetService<DbContextMapperCollection>();
            if (collection == null)
            {
                ThrowDetachedNotConfigured();
            }

            return collection.GetMapper(profileKey, dbContext);
        }

        public static DbContextMapper GetMapper(this DbContext dbContext)
        {
            DbContextMapperCollection profiles = dbContext.GetService<DbContextMapperCollection>();
            if (profiles == null)
            {
                ThrowDetachedNotConfigured();
            }

            return profiles.GetDefaultMapper(dbContext);
        }

        public static ITypeOptions GetTypeOptions(this DbContext dbContext, Type type)
        {
            return dbContext.GetMapper().MapperOptions.GetTypeOptions(type);
        }

        static void ThrowDetachedNotConfigured()
        {
            throw new ApplicationException($"Detached is not configured. Did you miss UseDetached or UseDetachedProfiles call?");
        }

        public static IQueryable<TProjection> Project<TEntity, TProjection>(this DbContext dbContext, IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return dbContext.GetMapper().QueryProvider.Project<TEntity, TProjection>(query);
        }

        public static Task<TEntity> MapAsync<TEntity>(this DbContext dbContext, object entityOrDTO, MapParameters parameters = null)
            where TEntity : class
        {
            Task<TEntity> task = Task.Run(() => Map<TEntity>(dbContext, entityOrDTO, parameters));
            task.ConfigureAwait(false);
            return task;
        }

        public static TEntity Map<TEntity>(this DbContext dbContext, object entityOrDTO, MapParameters parameters = null)
            where TEntity : class
        {
            DbContextMapper mapper = dbContext.GetMapper(); 

            if (parameters == null)
            {
                parameters = new MapParameters();
            }

            var context = new DbMapContext(dbContext, mapper.QueryProvider, parameters);

            return (TEntity)dbContext.GetMapper().Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), context);
        }

        public static async Task MapJsonAsync<TEntity>(this DbContext dbContext, Stream stream, MapParameters mapperParameters = null)
            where TEntity : class
        {
            if (mapperParameters == null)
            {
                mapperParameters = new MapParameters { AddAggregations = true };
            }
         
            JsonSerializerOptions jsonSerializerOptions = GetMapper(dbContext).JsonSerializerOptions;

            foreach (TEntity entity in await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(stream, jsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, entity, mapperParameters);
                }
            }
        }

        public static async Task MapJsonAsync<TEntity>(this DbContext dbContext, string json, MapParameters mapperParameters = null)
            where TEntity : class
        {
            if (mapperParameters == null)
            {
                mapperParameters = new MapParameters { AddAggregations = true };
            }

            JsonSerializerOptions jsonSerializerOptions = GetMapper(dbContext).JsonSerializerOptions;
         
            foreach (TEntity entity in JsonSerializer.Deserialize<IEnumerable<TEntity>>(json, jsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, entity, mapperParameters);
                }
            }
        }

        public static async Task MapJsonFileAsync<TEntity>(this DbContext dbContext, string filePath, MapParameters mapperParameters = null)
           where TEntity : class
        {
            using (Stream fileStream = File.OpenRead(filePath))
            {
                await MapJsonAsync<TEntity>(dbContext, fileStream, mapperParameters);
            }
        }

        public static async Task MapJsonResourceAsync<TEntity>(this DbContext dbContext, string resourceName, Assembly assembly = null,  MapParameters mapperParameters = null)
           where TEntity : class
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            using (Stream fileStream = assembly.GetManifestResourceStream(resourceName))
            {
                await MapJsonAsync<TEntity>(dbContext, fileStream, mapperParameters);
            }
        }
    }
}