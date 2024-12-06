using Detached.Mappers.Options;
using Detached.PatchTypes;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework.Options
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
    }
}