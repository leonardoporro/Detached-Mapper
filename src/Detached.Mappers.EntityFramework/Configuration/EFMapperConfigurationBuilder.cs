using System;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EFMapperConfigurationBuilder
    {
        internal Dictionary<object, Action<MapperOptions>> MapperConfigurations { get; } = new Dictionary<object, Action<MapperOptions>>();

        public EFMapperConfigurationBuilder()
        {
            
        }

        public EFMapperConfigurationBuilder AddProfile(object key, Action<MapperOptions> configure)
        {
            MapperConfigurations.Add(key, configure);
            return this;
        }

        public EFMapperConfigurationBuilder Default(Action<MapperOptions> configure)
        {
            return AddProfile(EFMapperProfiles.DEFAULT_PROFILE_KEY, configure);
        }
    }
}