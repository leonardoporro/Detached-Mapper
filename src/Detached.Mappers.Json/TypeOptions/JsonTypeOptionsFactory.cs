using Detached.Mappers.TypeOptions;
using System;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonTypeOptionsFactory : ITypeOptionsFactory
    {
        readonly ITypeOptions _nodeOptions = new JsonNodeTypeOptions();
        readonly ITypeOptions _arrayOptions = new JsonArrayTypeOptions();
        readonly ITypeOptions _objectOptions = new JsonObjectTypeOptions();
        readonly ITypeOptions _valueOptions = new JsonValueTypeOptions();

        public ITypeOptions Create(MapperOptions options, Type type)
        {
            if (typeof(JsonArray).IsAssignableFrom(type))
                return _arrayOptions;
            else if (typeof(JsonObject).IsAssignableFrom(type))
                return _objectOptions;
            else if (typeof(JsonValue).IsAssignableFrom(type))
                return _valueOptions;
            else if (typeof(JsonNode).IsAssignableFrom(type))
                return _nodeOptions;
            else
                return null;
        }
    }
}
