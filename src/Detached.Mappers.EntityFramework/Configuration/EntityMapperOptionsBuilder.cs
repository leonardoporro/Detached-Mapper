using System;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EntityMapperOptionsBuilder 
    {
        readonly Type _dbContextType;
        readonly EntityMapperServices _services;

        public EntityMapperOptionsBuilder(Type dbContextType, EntityMapperServices services)
        {
            _dbContextType = dbContextType;
            _services = services;    
        } 

        public EntityMapperOptionsBuilder ConfigureMapper(Action<MapperOptions> configure)
        {
            _services.AddConfigureAction(_dbContextType, null, configure);
            return this;
        }

        public EntityMapperOptionsBuilder ConfigureMapper(object profileKey, Action<MapperOptions> configure)
        {
            _services.AddConfigureAction(_dbContextType, profileKey, configure);
            return this;
        }


        [Obsolete("Please use ConfigureMapper().")]
        public EntityMapperOptionsBuilder AddProfile(object key, Action<MapperOptions> configure)
        {
            return ConfigureMapper(key, configure); 
        }

        [Obsolete("Please use ConfigureMapper().")]
        public EntityMapperOptionsBuilder Default(Action<MapperOptions> configure)
        {
            return ConfigureMapper(null, configure);
        }
    }
}