using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Profiles;
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
        readonly Dictionary<ProfileKey, Profile> _profiles = new();

        public EntityMapper(EntityMapperOptions options)
        {
            Options = options;

            foreach (var entry in options.MapperOptions)
            {
                _profiles.Add(entry.Key, new Profile(entry.Value));
            }
        }

        public EntityMapperOptions Options { get; }

        public Profile GetProfile(ProfileKey profileKey)
        {
            if (!_profiles.TryGetValue(profileKey, out var profile))
            {
                throw new MapperException($"Profile {profileKey.Value} not found.");
            }

            return profile;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query, ProfileKey profileKey)
           where TEntity : class
           where TProjection : class
        {
            var projection = GetProfile(profileKey).Mapper.Bind<TEntity, TProjection>();

            return query.Select(projection);
        }

        public async Task<TEntity> MapAsync<TEntity>(DbContext dbContext, ProfileKey profileKey, object entityOrDto, MapParameters parameters = null)
            where TEntity : class
        {
            var profile = GetProfile(profileKey);

            var target = await profile.Loader.LoadAsync(dbContext, typeof(TEntity), entityOrDto);

            return Map<TEntity>(dbContext, profile, entityOrDto, target, parameters);
        }

        public TEntity Map<TEntity>(DbContext dbContext, ProfileKey profileKey, object entityOrDto, MapParameters parameters = null)
           where TEntity : class
        {
            var profile = GetProfile(profileKey);

            var target = profile.Loader.Load(dbContext, typeof(TEntity), entityOrDto);

            return Map<TEntity>(dbContext, profile, entityOrDto, target, parameters);
        }

        TTarget Map<TTarget>(DbContext dbContext, Profile profile, object entityOrDto, object target, MapParameters parameters)
            where TTarget : class
        {
            if (parameters == null)
            {
                parameters = new MapParameters();
            }

            if (target == null && parameters.MissingRootBehavior != MissingRootBehavior.Create)
            {
                throw new MapperException($"Entity {typeof(TTarget)} does not exist and MissingRootBehavior is set to Throw.");
            }

            var mapContext = new EntityMapContext(Options, dbContext, parameters);

            return (TTarget)profile.Mapper.Map(entityOrDto, entityOrDto.GetType(), target, typeof(TTarget), mapContext);
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