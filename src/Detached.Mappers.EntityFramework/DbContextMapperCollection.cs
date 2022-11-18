using Detached.Mappers.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework
{
    internal class DbContextMapperCollection : Dictionary<object, DbContextMapper>
    {
        static internal string DEFAULT_PROFILE_KEY = "def";

        readonly ConcurrentDictionary<object, DbContextMapper> _mappers;
        readonly DetachedConfigurationBuilder _configBuilder;

        public DbContextMapperCollection(DetachedConfigurationBuilder configBuilder)
        {
            _configBuilder = configBuilder;
            _mappers = new ConcurrentDictionary<object, DbContextMapper>();
        }

        public DbContextMapper GetMapper(object profileKey, DbContext dbContext)
        {
            if (profileKey == null)
                throw new ArgumentException("profileKey is null.");

            return _mappers.GetOrAdd(profileKey, key =>
            {
                if (!_configBuilder.MapperOptions.TryGetValue(profileKey, out MapperOptions mapperOptions))
                {
                    throw new ArgumentException($"No profile found for key '{profileKey}'. Did you miss UseDetachedProfiles or AddProfile call?");
                }

                return new DbContextMapper(dbContext, mapperOptions);
            });
        }
        public DbContextMapper GetDefaultMapper(DbContext dbContext)
        {
            return GetMapper(DEFAULT_PROFILE_KEY, dbContext);
        }
    }
}