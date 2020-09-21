using Detached.EntityFramework.Context;
using Detached.EntityFramework.Conventions;
using Detached.EntityFramework.Queries;
using Detached.Mapping;
using Detached.Model;
using Detached.Patching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.EntityFramework
{
    public class DetachedDbContext : DbContext
    {
        public DetachedDbContext(DbContextOptions options)
            : base(options)
        {
            (Mapper, QueryProvider, JsonSerializerOptions) = CreateDependencies(GetType());
        }

        public DetachedDbContext(DbContextOptions options, Mapper mapper, QueryProvider queryProvider)
            : base(options)
        {
            Mapper = mapper;
            QueryProvider = queryProvider;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return QueryProvider.Project<TProjection, TEntity>(query);
        }

        protected virtual Mapper Mapper { get; }

        protected virtual QueryProvider QueryProvider { get; }

        protected virtual JsonSerializerOptions JsonSerializerOptions { get; }

        protected virtual (Mapper, QueryProvider, JsonSerializerOptions) CreateDependencies(Type type)
        {
            MapperModelOptions modelOptions = new MapperModelOptions();
            modelOptions.Conventions.Add(new IsEntityConvention(Model));
            OnMapperCreating(modelOptions);
            var injectableModelOptions = Options.Create(modelOptions);

            Mapper mapper = new Mapper(injectableModelOptions, new TypeMapFactory());
            QueryProvider queryProvider = new QueryProvider(mapper, Model);
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.AllowTrailingCommas = true;
            jsonSerializerOptions.IgnoreReadOnlyProperties = true;
            jsonSerializerOptions.IgnoreReadOnlyFields = true;
            jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jsonSerializerOptions.PropertyNameCaseInsensitive = false;
            jsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            jsonSerializerOptions.Converters.Add(new PatchJsonConverterFactory(injectableModelOptions));

            return (mapper, queryProvider, jsonSerializerOptions);
        }

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