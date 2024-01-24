using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, Action<EntityMapperOptionsBuilder> configure = null)
        {
            AddMappingExtension(dbContextBuilder, configure);

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseMapping<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<EntityMapperOptionsBuilder> configure = null)
            where TDbContext : DbContext
        {
            AddMappingExtension(dbContextBuilder, configure);

            return dbContextBuilder;
        }

        static void AddMappingExtension(DbContextOptionsBuilder dbContextBuilder, Action<EntityMapperOptionsBuilder> configure = null)
        {
            ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder).AddOrUpdateExtension(new EntityMapperDbContextOptionsExtension(dbContextBuilder.Options.ContextType, configure));
        }
    }
}