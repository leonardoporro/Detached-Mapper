using Detached.Mappers.HotChocolate.TypeMappers;
using Detached.Mappers.HotChocolate.Types;
using Detached.Mappers.Options;

namespace Detached.Mappers.HotChocolate
{
    public static class Package
    {
        public static MapperOptions WithHotChocolate(this MapperOptions mapperOptions)
        {
            mapperOptions.TypeMapperFactories.Add(new OptionalTypeMapperFactory());
            mapperOptions.TypeFactories.Add(new OptionalClassTypeFactory());

            return mapperOptions;
        }
    }
}
