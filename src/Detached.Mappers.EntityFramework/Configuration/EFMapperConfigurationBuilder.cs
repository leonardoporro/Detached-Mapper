using System;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EFMapperConfigurationBuilder
    {
        internal Dictionary<object, MapperOptions> MapperOptions { get; } = new Dictionary<object, MapperOptions>();

        public EFMapperConfigurationBuilder()
        {
            MapperOptions.Add(EFMapperServiceProvider.DEFAULT_PROFILE_KEY, new MapperOptions());
        }

        public EFMapperConfigurationBuilder AddProfile(object key, Action<MapperOptions> configure = null)
        {
            MapperOptions mapperOptions = new MapperOptions();
            configure?.Invoke(mapperOptions);

            MapperOptions.Add(key, mapperOptions);

            return this;
        }

        public EFMapperConfigurationBuilder Default(Action<MapperOptions> configure = null)
        {
            configure?.Invoke(MapperOptions[EFMapperServiceProvider.DEFAULT_PROFILE_KEY]);
            return this;
        }
    }
}