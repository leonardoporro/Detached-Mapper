using System;
using System.Text.Json.Nodes;
using Detached.Mappers.Options;

namespace Detached.Mappers.Types.Json
{
    public class JsonTypeFactory : ITypeFactory
    {
        readonly IType _nodeOptions = new JsonNodeType();
        readonly IType _arrayOptions = new JsonArrayType();
        readonly IType _objectOptions = new JsonObjectType();
        readonly IType _valueOptions = new JsonValueType();

        public IType Create(MapperOptions options, Type clrType)
        {
            if (typeof(JsonArray).IsAssignableFrom(clrType))
                return _arrayOptions;
            else if (typeof(JsonObject).IsAssignableFrom(clrType))
                return _objectOptions;
            else if (typeof(JsonValue).IsAssignableFrom(clrType))
                return _valueOptions;
            else if (typeof(JsonNode).IsAssignableFrom(clrType))
                return _nodeOptions;
            else
                return null;
        }
    }
}
