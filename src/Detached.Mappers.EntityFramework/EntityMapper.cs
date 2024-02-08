using Detached.Mappers.EntityFramework.Loaders;
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
        public EntityMapper(MapperOptions mapperOptions, JsonSerializerOptions jsonOptions)
        {
            MapperOptions = mapperOptions;
            JsonOptions = jsonOptions;
            Mapper = new Mapper(mapperOptions);
            Loader = new Loader(mapperOptions);
        }

        public MapperOptions MapperOptions { get; }

        public JsonSerializerOptions JsonOptions { get; }

        public Mapper Mapper { get; }

        public Loader Loader { get; }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
           where TEntity : class
           where TProjection : class
        {
            var projection = Mapper.Bind<TEntity, TProjection>();

            return query.Select(projection);
        }

        public async Task<TEntity> MapAsync<TEntity>(DbContext dbContext, object entityOrDto, MapParameters parameters = null)
            where TEntity : class
        {
            var target = await Loader.LoadAsync(dbContext, typeof(TEntity), entityOrDto);

            return Map<TEntity>(dbContext, entityOrDto, target, parameters);
        }

        public async Task<IEnumerable<TEntity>> MapAsync<TEntity>(DbContext dbContext, IEnumerable<object> entitiesOrDtos, MapParameters parameters = null)
            where TEntity : class
        {
            var targets = await Loader.LoadAsync(dbContext, typeof(TEntity), entitiesOrDtos);

            return Map<TEntity>(dbContext, entitiesOrDtos, targets, parameters);
        }

        public TEntity Map<TEntity>(DbContext dbContext, object entityOrDto, MapParameters parameters = null)
           where TEntity : class
        {
            var target = Loader.Load(dbContext, typeof(TEntity), entityOrDto);

            return Map<TEntity>(dbContext, entityOrDto, target, parameters);
        }

        IEnumerable<TTarget> Map<TTarget>(DbContext dbContext, IEnumerable<object> entitiesOrDtos, IEnumerable<object> targets, MapParameters parameters)
           where TTarget : class
        {
            if (parameters == null)
            {
                parameters = new MapParameters();
            }

            if (targets == null && parameters.MissingRootBehavior != MissingRootBehavior.Create)
            {
                throw new MapperException($"Entity {typeof(TTarget)} does not exist and MissingRootBehavior is set to Throw.");
            }

            var mapContext = new EntityMapContext(MapperOptions, dbContext, parameters);

            return (IEnumerable<TTarget>)Mapper.Map(entitiesOrDtos, entitiesOrDtos.GetType(), targets, typeof(IEnumerable<TTarget>), mapContext);
        }

        TTarget Map<TTarget>(DbContext dbContext, object entityOrDto, object target, MapParameters parameters)
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

            var mapContext = new EntityMapContext(MapperOptions, dbContext, parameters);

            return (TTarget)Mapper.Map(entityOrDto, entityOrDto.GetType(), target, typeof(TTarget), mapContext);
        }


        public async Task MapJsonAsync<TEntity>(DbContext dbContext, Stream stream, MapParameters mapParams = null)
            where TEntity : class
        {
            if (mapParams == null)
            {
                mapParams = new MapParameters
                {
                    MissingAggregationBehavior = MissingAggregationBehavior.Create
                };
            }

            foreach (TEntity entity in await JsonSerializer.DeserializeAsync<IEnumerable<TEntity>>(stream, JsonOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, entity, mapParams);
                }
            }
        }

        public async Task MapJsonAsync<TEntity>(DbContext dbContext, string json, MapParameters mapParams = null)
            where TEntity : class
        {
            if (mapParams == null)
            {
                mapParams = new MapParameters
                {
                    MissingAggregationBehavior = MissingAggregationBehavior.Create
                };
            }

            foreach (TEntity entity in JsonSerializer.Deserialize<IEnumerable<TEntity>>(json, JsonOptions))
            {
                if (entity != null)
                {
                    await MapAsync<TEntity>(dbContext, entity, mapParams);
                }
            }
        }

        public async Task MapJsonFileAsync<TEntity>(DbContext dbContext, string filePath, MapParameters mapParams = null)
           where TEntity : class
        {
            using (Stream fileStream = File.OpenRead(filePath))
            {
                await MapJsonAsync<TEntity>(dbContext, fileStream, mapParams);
            }
        }

        public async Task MapJsonResourceAsync<TEntity>(DbContext dbContext, string resourceName, Assembly assembly = null, MapParameters mapParams = null)
           where TEntity : class
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            using (Stream fileStream = assembly.GetManifestResourceStream(resourceName))
            {
                await MapJsonAsync<TEntity>(dbContext, fileStream, mapParams);
            }
        }
    }
}