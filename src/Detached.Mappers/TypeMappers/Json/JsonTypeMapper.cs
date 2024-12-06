using Detached.Mappers.Context;
using Detached.Mappers.TypeMappers;
using System.Text.Json.Nodes;

namespace Detached.Mappers.TypeMappers.Json
{
    public class JsonTypeMapper<TTarget> : TypeMapper<JsonValue, TTarget>
    {
        public override TTarget Map(JsonValue source, TTarget target, IMapContext context)
        {
            source.TryGetValue(out TTarget result);
            return result;
        }
    }
}
