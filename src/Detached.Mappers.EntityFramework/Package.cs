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
            UseMapper(dbContextBuilder, cfg => cfg.DefaultProfile(configure));
            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseDetached<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<MapperOptions> configure = null)
            where TDbContext : DbContext
        {
            UseMapper(dbContextBuilder, cfg => cfg.DefaultProfile(configure));
            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapper(this DbContextOptionsBuilder dbContextBuilder, Action<EFMapperConfigurationBuilder> configure = null)
        {
            EFMapperConfigurationBuilder builder = new EFMapperConfigurationBuilder();

            configure?.Invoke(builder);

            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EFMapperDbContextOptionsExtension(builder));

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseMapper<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<EFMapperConfigurationBuilder> configure = null)
            where TDbContext : DbContext
        {
            EFMapperConfigurationBuilder builder = new EFMapperConfigurationBuilder();

            configure?.Invoke(builder);

            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EFMapperDbContextOptionsExtension(builder));

            return dbContextBuilder;
        }
    }
}