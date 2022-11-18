using System;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Extensions
{
    public class DetachedConfigurationBuilder
    {
        internal Dictionary<object, MapperOptions> MapperOptions { get; } = new Dictionary<object, MapperOptions>();

        public DetachedConfigurationBuilder()
        {
            MapperOptions.Add(DbContextMapperCollection.DEFAULT_PROFILE_KEY, new MapperOptions());
        }

        public DetachedConfigurationBuilder AddProfile(object key, Action<MapperOptions> configure = null)
        {
            MapperOptions mapperOptions = new MapperOptions();
            configure?.Invoke(mapperOptions);

            MapperOptions.Add(key, mapperOptions);

            return this;
        }

        public DetachedConfigurationBuilder DefaultProfile(Action<MapperOptions> configure = null)
        {
            configure?.Invoke(MapperOptions[DbContextMapperCollection.DEFAULT_PROFILE_KEY]);
            return this;
        }
    }
}