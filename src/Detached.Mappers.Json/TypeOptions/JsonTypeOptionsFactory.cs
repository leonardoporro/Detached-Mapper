using Detached.Mappers.Types;
using System;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonTypeOptionsFactory : ITypeFactory
    {
        readonly IType _nodeOptions = new JsonNodeTypeOptions();
        readonly IType _arrayOptions = new JsonArrayTypeOptions();
        readonly IType _objectOptions = new JsonObjectTypeOptions();
        readonly IType _valueOptions = new JsonValueTypeOptions();

        public IType Create(MapperOptions options, Type type)
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
