using Detached.Mappers.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Detached.Mappers.Patching
{
    public class PatchJsonConverterFactory : JsonConverterFactory
    {
        readonly MapperOptions _options;

        public PatchJsonConverterFactory(MapperOptions options)
        {
            _options = options;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return _options.GetTypeOptions(typeToConvert).IsComplexType 
                && !typeof(IPatch).IsAssignableFrom(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(typeof(PatchJsonConverter<>).MakeGenericType(typeToConvert));
        }
    }
}