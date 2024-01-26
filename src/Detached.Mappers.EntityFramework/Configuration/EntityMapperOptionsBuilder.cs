using Detached.Mappers.EntityFramework.Profiles;
using Detached.PatchTypes;
using System;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EntityMapperOptionsBuilder
    {
        public EntityMapperOptionsBuilder(EntityMapperOptions options = null)
        {
            Options = options ?? new EntityMapperOptions();

            Options.JsonSerializerOptions = new JsonSerializerOptions();
            Options.JsonSerializerOptions.AllowTrailingCommas = true;
            Options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
            Options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            Options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            Options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            Options.JsonSerializerOptions.Converters.Add(new PatchJsonConverterFactory());
          
            Options.MapperOptions.TryAdd(new ProfileKey(null), new MapperOptions());
        }

        public EntityMapperOptions Options { get; }

        public EntityMapperOptionsBuilder AddProfile(object profileKey, Action<MapperOptions> configure)
        {
            var mapperOptions = Options.MapperOptions.GetOrAdd(new ProfileKey(profileKey), key => new MapperOptions());

            configure?.Invoke(mapperOptions);

            return this;
        }

        public EntityMapperOptionsBuilder Default(Action<MapperOptions> configure)
        {
            var mapperOptions = Options.MapperOptions.GetOrAdd(ProfileKey.Empty, key => new MapperOptions());

            configure?.Invoke(mapperOptions);

            return this;
        }
    }
}