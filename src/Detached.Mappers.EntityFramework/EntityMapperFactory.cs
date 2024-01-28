using Detached.Mappers.EntityFramework.Conventions;
using Detached.Mappers.EntityFramework.TypeMappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapperFactory
    {
        readonly ConcurrentDictionary<Type, EntityMapperOptions> _allOptions = new();
        readonly ConcurrentDictionary<EntityMapperKey, EntityMapper> _mappers = new();

        public virtual void Configure(Type dbContexType, EntityMapperOptions options)
        {
            _allOptions.TryAdd(dbContexType, options);
        }

        public EntityMapper CreateMapper(DbContext dbContext, object profileKey)
        {
            var key = new EntityMapperKey(dbContext.GetType(), profileKey);

            return _mappers.GetOrAdd(key, key =>
            {
                if (!(_allOptions.TryGetValue(key.DbContextType, out EntityMapperOptions entityMapperOptions)
                     || _allOptions.TryGetValue(typeof(DbContext), out entityMapperOptions)))
                {
                    entityMapperOptions = new EntityMapperOptions();
                }

                ApplyOnMapperCreating(dbContext, entityMapperOptions);

                MapperOptions mapperOptions;

                if (key.ProfileKey != null)
                    mapperOptions = entityMapperOptions.Profiles[key.ProfileKey];
                else
                    mapperOptions = entityMapperOptions;

                mapperOptions.TypeConventions.Add(new EntityTypeConventions(dbContext.Model));
                mapperOptions.TypeMapperFactories.Add(new EntityTypeMapperFactory());
                mapperOptions.TypeMapperFactories.Add(new KeyToEntityTypeMapperFactory());

                return new EntityMapper(mapperOptions, entityMapperOptions.JsonOptions);
            });
        }

        static void ApplyOnMapperCreating(DbContext dbContext, EntityMapperOptions options)
        {
            MethodInfo configureMapperMethodInfo = dbContext.GetType().GetMethod("OnMapperCreating");
            if (configureMapperMethodInfo != null)
            {
                var parameters = configureMapperMethodInfo.GetParameters();
                if (parameters.Length != 1 && parameters[0].ParameterType != typeof(EntityMapperOptions))
                {
                    throw new ArgumentException($"OnMapperCreating method must have a single argument of type {nameof(EntityMapperOptions)}");
                }

                configureMapperMethodInfo.Invoke(dbContext, new[] { options });
            }
        }
    }
}