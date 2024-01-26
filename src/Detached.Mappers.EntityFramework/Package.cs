using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, Action<EntityMapperOptionsBuilder> configure = null)
        {
            var builder = new EntityMapperOptionsBuilder();

            configure?.Invoke(builder);

            UseMapping(dbContextBuilder, builder.Options);

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, IEnumerable<IEntityMapperConfiguration> configs)
        {
            var builder = new EntityMapperOptionsBuilder();

            foreach (var config in configs)
            {
                config.Apply(builder);
            }

            UseMapping(dbContextBuilder, builder.Options);

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, EntityMapperOptions options)
        {
            var builder = ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder);
            
            builder.AddOrUpdateExtension(new EntityMapperDbContextOptionsExtension(dbContextBuilder.Options.ContextType, options));

            return dbContextBuilder;
        }
    }
}