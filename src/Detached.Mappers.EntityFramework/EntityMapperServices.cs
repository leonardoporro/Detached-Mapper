using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapperServices
    {
        readonly ConcurrentDictionary<EntityMapperKey, List<Action<MapperOptions>>> _configureActions = new();
        readonly ConcurrentDictionary<EntityMapperKey, MapperOptions> _mapperOptions = new();
        readonly ConcurrentDictionary<EntityMapperKey, EntityMapper> _mappers = new();

        public void AddConfigureAction(Type dbContextType, object profileKey, Action<MapperOptions> configureAction)
        {
            _configureActions.AddOrUpdate(new EntityMapperKey(dbContextType, profileKey),
                key => new List<Action<MapperOptions>> { configureAction },
                (key, actions) =>
                {
                    actions.Add(configureAction);
                    return actions;
                });
        }

        public MapperOptions GetOptions(DbContext dbContext, EntityMapperKey mapperKey)
        {
            return _mapperOptions.GetOrAdd(mapperKey, key =>
            {
                MapperOptions mapperOptions = new MapperOptions();
                mapperOptions.TypeConventions.Add(new EntityTypeConventions(dbContext.Model));

                foreach (IEntityMapperCustomizer customizer in dbContext.GetInfrastructure().GetServices<IEntityMapperCustomizer>())
                {
                    customizer.Customize(dbContext, mapperKey.ProfileKey, mapperOptions);
                }

                CallOnCreateMappingMethod(dbContext);

                if (_configureActions.TryRemove(mapperKey, out List<Action<MapperOptions>> configureActions))
                {
                    foreach (var configureAction in configureActions)
                    {
                        configureAction(mapperOptions);
                    }
                }
                else if (mapperKey.ProfileKey != null)
                {
                    throw new ArgumentException($"No profile found for key '{mapperKey.ProfileKey}'. Did you miss UserMapper or AddProfile call?");
                }

                return mapperOptions;
            });
        }

        public EntityMapper GetMapper(DbContext dbContext, object profileKey)
        {
            var mapperKey = new EntityMapperKey(dbContext.GetType(), profileKey);

            return _mappers.GetOrAdd(mapperKey, key =>
            {
                var concurrencyTokens = GetConcurrencyTokens(dbContext);
                var options = GetOptions(dbContext, mapperKey);

                return new EntityMapper(options, concurrencyTokens);
            });
        }

        Dictionary<string, string> GetConcurrencyTokens(DbContext dbContext)
        {
            var result = new Dictionary<string, string>();

            foreach (IEntityType entityType in dbContext.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.IsConcurrencyToken)
                    {
                        result.Add(entityType.Name, property.Name);
                    }
                }
            }

            return result;
        }

        public void CallOnCreateMappingMethod(DbContext dbContext)
        {
            MethodInfo configureMapperMethodInfo = dbContext.GetType().GetMethod("OnMapperCreating");
            if (configureMapperMethodInfo != null)
            {
                var parameters = configureMapperMethodInfo.GetParameters();
                if (parameters.Length != 1 && parameters[0].ParameterType != typeof(MapperOptions))
                {
                    throw new ArgumentException($"ConfigureMapper method must have a single argument of type MapperOptions");
                }

                EntityMapperOptionsBuilder builder = new EntityMapperOptionsBuilder(dbContext.GetType(), this);

                configureMapperMethodInfo.Invoke(dbContext, new[] { builder });
            }
        }
    }
}
