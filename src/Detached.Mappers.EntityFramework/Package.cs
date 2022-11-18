using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        public static DbContextOptionsBuilder UseDetached(this DbContextOptionsBuilder dbContextBuilder, Action<MapperOptions> configure = null)
        {
            UseMapping(dbContextBuilder, cfg => cfg.Default(configure));
            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseDetached<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<MapperOptions> configure = null)
            where TDbContext : DbContext
        {
            UseMapping(dbContextBuilder, cfg => cfg.Default(configure));
            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, Action<EFMapperConfigurationBuilder> configure = null)
        {
            EFMapperConfigurationBuilder builder = new EFMapperConfigurationBuilder();

            configure?.Invoke(builder);

            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EFMapperDbContextOptionsExtension(builder));

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseMapping<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<EFMapperConfigurationBuilder> configure = null)
            where TDbContext : DbContext
        {
            EFMapperConfigurationBuilder builder = new EFMapperConfigurationBuilder();

            configure?.Invoke(builder);

            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EFMapperDbContextOptionsExtension(builder));

            return dbContextBuilder;
        }
    }
}