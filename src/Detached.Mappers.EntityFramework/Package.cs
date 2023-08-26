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
            UseMapping(dbContextBuilder, cfg => cfg.Default(configure));
            return dbContextBuilder;
        }

        [Obsolete("Call UseMapping instead.")]
        public static DbContextOptionsBuilder<TDbContext> UseDetached<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<MapperOptions> configure = null)
            where TDbContext : DbContext
        {
            UseMapping(dbContextBuilder, cfg => cfg.Default(configure));
            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, Action<EFMapperConfigurationBuilder> configure = null)
        {
            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EFMapperDbContextOptionsExtension(configure));

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseMapping<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<EFMapperConfigurationBuilder> configure = null)
            where TDbContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EFMapperDbContextOptionsExtension(configure));

            return dbContextBuilder;
        }
    }
}