using Detached.PatchTypes;
using System;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapperOptions : MapperOptions
    {
        public EntityMapperOptions()
        {
            JsonOptions = new JsonSerializerOptions();
            JsonOptions.AllowTrailingCommas = true;
            JsonOptions.IgnoreReadOnlyProperties = true;
            JsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            JsonOptions.PropertyNameCaseInsensitive = false;
            JsonOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            JsonOptions.Converters.Add(new PatchJsonConverterFactory());
        }

        public ConcurrentDictionary<object, MapperOptions> Profiles { get; } = new();

        public JsonSerializerOptions JsonOptions { get; set; }

        public EntityMapperOptions AddProfile(object profileKey, Action<MapperOptions> configure)
        {
            var mapperOptions = Profiles.GetOrAdd(profileKey, key => new MapperOptions());

            configure?.Invoke(mapperOptions);

            return this;
        }

        public EntityMapperOptions Default(Action<MapperOptions> configure)
        {
            configure?.Invoke(this);

            return this;
        }
    }
}
