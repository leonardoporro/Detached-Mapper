using Detached.Mappers.HotChocolate.TypeMappers.POCO.Optional;

namespace Detached.Mappers.HotChocolate
{
    public static class Package
    {
        public static MapperOptions WithHotChocolate(this MapperOptions mapperOptions)
        {
            mapperOptions.TypeMapperFactories.Add(new OptionalTypeMapperFactory());
            return mapperOptions;
        }
    }
}
