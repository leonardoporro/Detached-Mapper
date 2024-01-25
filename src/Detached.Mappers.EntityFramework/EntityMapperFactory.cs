using Detached.Mappers.EntityFramework.Configuration;
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
        readonly ConcurrentDictionary<Type, EntityMapper> _mappers = new();

        public virtual void Configure(Type dbContexType, EntityMapperOptions options)
        {
            _allOptions.TryAdd(dbContexType, options);
        }

        public EntityMapper CreateMapper(DbContext dbContext)
        {
            return _mappers.GetOrAdd(dbContext.GetType(), key =>
            {
                if (!(_allOptions.TryGetValue(dbContext.GetType(), out EntityMapperOptions options)
                     || _allOptions.TryGetValue(typeof(DbContext), out options)))
                {
                    options = new EntityMapperOptions();
                }

                var builder = new EntityMapperOptionsBuilder(options);

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

                foreach (var entry in builder.Options.MapperOptions)
                {
                    entry.Value.TypeConventions.Add(new EntityTypeConventions(dbContext.Model));
                    entry.Value.TypeMapperFactories.Add(new EntityTypeMapperFactory());
                }

                return new EntityMapper(builder.Options);
            });
        }
    }
}