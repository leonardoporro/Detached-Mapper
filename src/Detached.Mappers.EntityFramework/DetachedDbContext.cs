using Detached.Mappers.EntityFramework.Context;
using Detached.Mappers.EntityFramework.Conventions;
using Detached.Mappers.EntityFramework.Queries;
using Detached.Mappers.Model;
using Detached.Mappers.Patching;
using Detached.Mappers.TypeMaps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework
{
    public class DetachedDbContext : DbContext
    {
        static ConcurrentDictionary<Type, DetachedDbContextDependencies> _dependencyCache =
            new ConcurrentDictionary<Type, DetachedDbContextDependencies>();

        public static DetachedDbContextDependencies GetDependencies(DetachedDbContext dbContext)
        {
            return _dependencyCache.GetOrAdd(dbContext.GetType(), type =>
            {
                MapperModelOptions modelOptions = new MapperModelOptions();
                modelOptions.Conventions.Add(new IsEntityConvention(dbContext.Model));
                dbContext.OnMapperCreating(modelOptions);

                var injectableModelOptions = Options.Create(modelOptions);

                Mapper mapper = new Mapper(injectableModelOptions, new TypeMapFactory());
                QueryProvider queryProvider = new QueryProvider(mapper, dbContext.Model);
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.AllowTrailingCommas = true;
                jsonSerializerOptions.IgnoreReadOnlyProperties = true;
                jsonSerializerOptions.IgnoreReadOnlyFields = true;
                jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                jsonSerializerOptions.PropertyNameCaseInsensitive = false;
                jsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                jsonSerializerOptions.Converters.Add(new PatchJsonConverterFactory(injectableModelOptions));

                return new DetachedDbContextDependencies(type, mapper, jsonSerializerOptions, queryProvider);
            });
        }

        public DetachedDbContext(DbContextOptions options)
            : base(options)
        {
            var deps = GetDependencies(this);
            Mapper = deps.Mapper;
            QueryProvider = deps.QueryProvider;
            JsonSerializerOptions = deps.SerializerOptions;
        }
 
        public DetachedDbContext(DbContextOptions options, Mapper mapper, QueryProvider queryProvider, JsonSerializerOptions jsonSerializerOptions)
            : base(options)
        {
            Mapper = mapper;
            QueryProvider = queryProvider; 
            JsonSerializerOptions = jsonSerializerOptions;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return QueryProvider.Project<TProjection, TEntity>(query);
        }

        public virtual Mapper Mapper { get; }

        public virtual QueryProvider QueryProvider { get; }

        public virtual JsonSerializerOptions JsonSerializerOptions { get; }

        protected virtual void OnMapperCreating(MapperModelOptions options)
        {
        }

        public Task<TEntity> MapAsync<TEntity>(object entityOrDTO, MappingOptions mapperOptions = null)
            where TEntity : class
        {
            return Task.Run(() => Map<TEntity>(entityOrDTO, mapperOptions));
        }

        public TEntity Map<TEntity>(object entityOrDTO, MappingOptions mapperOptions = null)
            where TEntity : class
        {
            if (mapperOptions == null)
                mapperOptions = new MappingOptions();

            var context = new EntityFrameworkMapperContext(this, QueryProvider, mapperOptions);

            return (TEntity)Mapper.Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), context);
        }

        public async Task ImportJsonAsync<TEntity>(Stream stream)
            where TEntity : class
        {
            foreach (TEntity entity in await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(stream, JsonSerializerOptions))
            {
                if (entity != null)
                {
                    var result = await MapAsync<TEntity>(entity, new MappingOptions { EnsureAggregations = true });
                }
            }
        }

        public async Task ImportJsonAsync<TEntity>(string json)
            where TEntity : class
        {
            foreach (TEntity entity in JsonSerializer.Deserialize<IEnumerable<TEntity>>(json, JsonSerializerOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(entity, new MappingOptions { EnsureAggregations = true });
                }
            }
        }
    }
}