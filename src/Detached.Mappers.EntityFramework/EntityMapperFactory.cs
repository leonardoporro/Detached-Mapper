using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapperFactory
    {
        readonly ConcurrentDictionary<Type, Action<EntityMapperOptionsBuilder>> _configureActions = new();
        readonly ConcurrentDictionary<Type, EntityMapperOptions> _options = new();
        readonly ConcurrentDictionary<Type, EntityMapper> _mappers = new();

        public virtual void RegisterConfigureAction(Type dbContexType, Action<EntityMapperOptionsBuilder> configureAction)
        {
            _configureActions.TryAdd(dbContexType, configureAction);
        }

        public EntityMapper CreateMapper(DbContext dbContext)
        {
            var options = _options.GetOrAdd(dbContext.GetType(), key =>
            {
                var builder = new EntityMapperOptionsBuilder(dbContext);

                if (_configureActions.TryRemove(key, out var configureAction))
                {
                    configureAction(builder);
                }

                if (_configureActions.TryRemove(typeof(DbContext), out var genericConfigureAction))
                {
                    genericConfigureAction(builder);
                }

                foreach (IEntityMapperCustomizer customizer in dbContext.GetInfrastructure().GetServices<IEntityMapperCustomizer>())
                {
                    customizer.Customize(builder);
                }

                MethodInfo configureMapperMethodInfo = dbContext.GetType().GetMethod("OnMapperCreating");
                if (configureMapperMethodInfo != null)
                {
                    var parameters = configureMapperMethodInfo.GetParameters();
                    if (parameters.Length != 1 && parameters[0].ParameterType != typeof(EntityMapperOptionsBuilder))
                    {
                        throw new ArgumentException($"OnMapperCreating method must have a single argument of type {nameof(EntityMapperOptionsBuilder)}");
                    }

                    configureMapperMethodInfo.Invoke(dbContext, new[] { builder });
                }

                return builder.Options;
            });

            return _mappers.GetOrAdd(dbContext.GetType(), key => new EntityMapper(options));

        }
    }
}