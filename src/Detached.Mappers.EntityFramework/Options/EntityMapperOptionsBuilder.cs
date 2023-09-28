using Detached.Mappers.EntityFramework.Conventions;
using Detached.Mappers.EntityFramework.Profiles;
using Detached.Mappers.EntityFramework.TypeMappers;
using Detached.PatchTypes;
using Microsoft.EntityFrameworkCore;
using System;
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
            mapperOptions.TypeMapperFactories.Add(new EntityTypeMapperFactory());

            return mapperOptions;
        }
    }
}