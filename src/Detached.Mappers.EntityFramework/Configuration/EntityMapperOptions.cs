using Detached.Mappers.EntityFramework.Profiles;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EntityMapperOptions
    {
        public ConcurrentDictionary<ProfileKey, MapperOptions> MapperOptions { get; } = new();

        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}
