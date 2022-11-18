using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Extensions;
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

        public static DbContextOptionsBuilder UseMapper(this DbContextOptionsBuilder dbContextBuilder, Action<DetachedConfigurationBuilder> configure = null)
        {
            DetachedConfigurationBuilder builder = new DetachedConfigurationBuilder();

            configure?.Invoke(builder);

            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new DetachedDbContextOptionsExtension(builder));

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseMapper<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<DetachedConfigurationBuilder> configure = null)
            where TDbContext : DbContext
        {
            DetachedConfigurationBuilder builder = new DetachedConfigurationBuilder();

            configure?.Invoke(builder);

            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new DetachedDbContextOptionsExtension(builder));

            return dbContextBuilder;
        }
    }
}