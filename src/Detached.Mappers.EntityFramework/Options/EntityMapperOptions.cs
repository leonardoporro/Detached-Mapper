using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using Detached.Mappers.EntityFramework.Profiles;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EntityMapperOptions
    {
        public ConcurrentDictionary<ProfileKey, MapperOptions> MapperOptions { get; } = new();

        public Dictionary<string, string> ConcurrencyTokens { get; set; }

        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}
