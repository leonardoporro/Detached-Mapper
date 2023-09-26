using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Queries;
using Detached.Mappers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapper
    {
        readonly Dictionary<ProfileKey, Mapper> _mappers = new();
        readonly Dictionary<ProfileKey, QueryProvider> _queryProviders = new();

        public EntityMapper(EntityMapperOptions options)
        {
            Options = options;

            foreach (var entry in options.MapperOptions)
            {
                _mappers.Add(entry.Key, new Mapper(entry.Value));
                _queryProviders.Add(entry.Key, new QueryProvider(entry.Value));
            }
        }

        public EntityMapperOptions Options { get; }

        public Mapper GetMapper(ProfileKey profileKey)
        {
            if (!_mappers.TryGetValue(profileKey, out var mapper))
            {
                throw new MapperException($"Profile {profileKey.Value} not found.");
            }

            return mapper;
        }

        public QueryProvider GetQueryProvider(ProfileKey profileKey)
        {
            if (!_queryProviders.TryGetValue(profileKey, out var mapper))
            {
                throw new MapperException($"Profile {profileKey.Value} not found.");
            }

            return mapper;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query, ProfileKey profileKey)
           where TEntity : class
           where TProjection : class
        {
            var projection = GetMapper(profileKey).Bind<TEntity, TProjection>();

            return query.Select(projection);
        }

        public Task<TEntity> MapAsync<TEntity>(DbContext dbContext, ProfileKey profileKey, object entityOrDTO, MapParameters parameters = null)
            where TEntity : class
        {
            var task = Task.Run(() => Map<TEntity>(dbContext, profileKey, entityOrDTO, parameters));
            task.ConfigureAwait(false);
            return task;
        }

        public TEntity Map<TEntity>(DbContext dbContext, ProfileKey profileKey, object entityOrDTO, MapParameters parameters = null)
            where TEntity : class
        {
            if (parameters == null)
            {
                parameters = new MapParameters();
            }

            var context = new EntityMapContext(Options, dbContext, GetQueryProvider(profileKey), parameters);

            return (TEntity)GetMapper(profileKey).Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), context);
        }

        public async Task MapJsonAsync<TEntity>(DbContext dbContext, ProfileKey profileKey, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            if (mapParams == null)
            {
                mapParams = new MapParameters
                {
                    MissingAggregationBehavior = MissingAggregationBehavior.Create
                };
            }

            foreach (TEntity entity in await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(stream, Options.JsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, profileKey, entity, mapParams);
                }
            }
        }

        public async Task MapJsonAsync<TEntity>(DbContext dbContext, ProfileKey profileKey, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            if (mapParams == null)
            {
                mapParams = new MapParameters
                {
                    MissingAggregationBehavior = MissingAggregationBehavior.Create
                };
            }

            foreach (TEntity entity in JsonSerializer.Deserialize<IEnumerable<TEntity>>(json, Options.JsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, profileKey, entity, mapParams);
                }
            }
        }

        public async Task MapJsonFileAsync<TEntity>(DbContext dbContext, ProfileKey profileKey, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            using (Stream fileStream = File.OpenRead(filePath))
            {
                await MapJsonAsync<TEntity>(dbContext, profileKey, fileStream, mapParams);
            }
        }

        public async Task MapJsonResourceAsync<TEntity>(DbContext dbContext, ProfileKey profileKey, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            using (Stream fileStream = assembly.GetManifestResourceStream(resourceName))
            {
                await MapJsonAsync<TEntity>(dbContext, profileKey, fileStream, mapParams);
            }
        }
    }
}