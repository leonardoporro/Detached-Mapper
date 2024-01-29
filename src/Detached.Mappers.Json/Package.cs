using Detached.Mappers.Json.TypeMappers;
using Detached.Mappers.Json.TypeOptions;

namespace Detached.Mappers.Json
{
    public static class Package
    {
        public static MapperOptions WithJson(this MapperOptions mapperOptions)
        {
            mapperOptions.TypeFactories.Add(new JsonTypeFactory());
            mapperOptions.TypeMapperFactories.Add(new JsonTypeMapperFactory());
            return mapperOptions;
        }
    }
}
