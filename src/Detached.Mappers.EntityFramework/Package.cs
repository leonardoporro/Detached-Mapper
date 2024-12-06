using Detached.Mappers.EntityFramework.Integration;
using Detached.Mappers.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, Action<EntityMapperOptions> configure = null)
        {
            var options = new EntityMapperOptions();

            configure?.Invoke(options);

            UseMapping(dbContextBuilder, options);

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