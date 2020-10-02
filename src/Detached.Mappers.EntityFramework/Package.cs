using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        public static DbContextOptionsBuilder UseDetached(this DbContextOptionsBuilder builder, Action<MapperOptions> configure = null)
        {
            MapperOptions mapperOptions = new MapperOptions();
            configure?.Invoke(mapperOptions);

            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new DetachedDbContextOptionsExtension(mapperOptions));
            return builder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseDetached<TDbContext>(this DbContextOptionsBuilder<TDbContext> builder, Action<MapperOptions> configure = null)
            where TDbContext : DbContext
        {

            MapperOptions mapperOptions = new MapperOptions();
            configure?.Invoke(mapperOptions);

            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new DetachedDbContextOptionsExtension(mapperOptions));
            return builder;
        }
    }
}