using Detached.Mappers.EntityFramework.Conventions;
using Detached.PatchTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EntityMapperOptionsBuilder
    {
        readonly DbContext _dbContext;

        public EntityMapperOptionsBuilder(DbContext dbContext)
        {
            _dbContext = dbContext;
            Options = new EntityMapperOptions();

            Options.JsonSerializerOptions = new JsonSerializerOptions();
            Options.JsonSerializerOptions.AllowTrailingCommas = true;
            Options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
            Options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            Options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            Options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            Options.JsonSerializerOptions.Converters.Add(new PatchJsonConverterFactory());
            
            Options.ConcurrencyTokens = GetConcurrencyTokens();
            Options.MapperOptions.TryAdd(new ProfileKey(null), CreateMapperOptions());
        }

        public EntityMapperOptions Options { get; }

        public EntityMapperOptionsBuilder AddProfile(object profileKey, Action<MapperOptions> configure)
        {
            var mapperOptions = Options.MapperOptions.GetOrAdd(new ProfileKey(profileKey), key => CreateMapperOptions());

            configure?.Invoke(mapperOptions);

            return this;
        }

        public EntityMapperOptionsBuilder Default(Action<MapperOptions> configure)
        {
            var mapperOptions = Options.MapperOptions.GetOrAdd(ProfileKey.Empty, key => CreateMapperOptions());

            configure?.Invoke(mapperOptions);

            return this;
        }

        MapperOptions CreateMapperOptions()
        {
            var mapperOptions = new MapperOptions();
            mapperOptions.TypeConventions.Add(new EntityTypeConventions(_dbContext.Model));
            return mapperOptions;
        }

        Dictionary<string, string> GetConcurrencyTokens()
        {
            var result = new Dictionary<string, string>();

            foreach (IEntityType entityType in _dbContext.Model.GetEntityTypes())
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
    }
}