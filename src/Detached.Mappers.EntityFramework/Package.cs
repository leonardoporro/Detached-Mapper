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
            var builder = new EntityMapperOptionsBuilder();

            configure?.Invoke(builder);

            AddMappingExtension(dbContextBuilder, builder.Options);

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder<TDbContext> UseMapping<TDbContext>(this DbContextOptionsBuilder<TDbContext> dbContextBuilder, Action<EntityMapperOptionsBuilder> configure = null)
            where TDbContext : DbContext
        {
            var builder = new EntityMapperOptionsBuilder();

            configure?.Invoke(builder);

            AddMappingExtension(dbContextBuilder, builder.Options);

            return dbContextBuilder;
        }

        static void AddMappingExtension(DbContextOptionsBuilder dbContextBuilder, EntityMapperOptions options)
        {
            var builder = ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder);
            
            builder.AddOrUpdateExtension(new EntityMapperDbContextOptionsExtension(dbContextBuilder.Options.ContextType, options));
        }
    }
}