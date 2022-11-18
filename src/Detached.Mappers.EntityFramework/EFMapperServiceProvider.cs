using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework
{
    internal class EFMapperServiceProvider : Dictionary<object, EFMapperServices>
    {
        static internal string DEFAULT_PROFILE_KEY = "def";

        readonly ConcurrentDictionary<object, EFMapperServices> _mappers;
        readonly EFMapperConfigurationBuilder _configBuilder;

        public EFMapperServiceProvider(EFMapperConfigurationBuilder configBuilder)
        {
            _configBuilder = configBuilder;
            _mappers = new ConcurrentDictionary<object, EFMapperServices>();
        }

        public EFMapperServices GetMapperServices(object profileKey, DbContext dbContext)
        {
            if (profileKey == null)
            {
                profileKey = DEFAULT_PROFILE_KEY;
            }

            return _mappers.GetOrAdd(profileKey, key =>
            {
                if (!_configBuilder.MapperOptions.TryGetValue(profileKey, out MapperOptions mapperOptions))
                {
                    throw new ArgumentException($"No profile found for key '{profileKey}'. Did you miss UseDetachedProfiles or AddProfile call?");
                }

                return new EFMapperServices(dbContext, mapperOptions);
            });
        }
    }
}