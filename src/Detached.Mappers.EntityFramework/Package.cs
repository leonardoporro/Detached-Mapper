using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        [Obsolete("Call UseMapping instead.")]
        public static DbContextOptionsBuilder UseDetached(this DbContextOptionsBuilder dbContextBuilder, Action<MapperOptions> configure = null)
        {
            UseMapping(dbContextBuilder);
            return dbContextBuilder;
        }

        [Obsolete("Call UseMapping instead.")]
        public static DbContextOptionsBuilder<TDbContext> UseDetached<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<MapperOptions> configure = null)
            where TDbContext : DbContext
        {
            UseMapping(dbContextBuilder);
            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, Action<EntityMapperOptionsBuilder> configure = null)
        {
            EntityMapperServices services = new EntityMapperServices();

            if (configure != null)
            {
                EntityMapperOptionsBuilder builder = new EntityMapperOptionsBuilder(dbContextBuilder.Options.ContextType, services);
                configure(builder);
            }

            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EntityMapperDbContextOptionsExtension(services));

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseMapping<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<EntityMapperOptionsBuilder> configure = null)
            where TDbContext : DbContext
        {
            UseMapping(dbContextBuilder, configure); 

            return dbContextBuilder;
        }
    }
}