using Detached.Mappers.EntityFramework.Integration;
using Detached.Mappers.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, Action<EntityMapperOptionsBuilder>? configure = null)
        {
            var options = new EntityMapperOptions();

            configure?.Invoke(new EntityMapperOptionsBuilder());

            UseMapping(dbContextBuilder, options);

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, EntityMapperOptions options)
        {
            var builder = ((IDbContextOptionsBuilderInfrastructure)dbContextBuilder);
            
            builder.AddOrUpdateExtension(new EntityMapperDbContextOptionsExtension(dbContextBuilder.Options.ContextType, options));

            return dbContextBuilder;
        }

        public static DbContextOptionsBuilder UseMapping(this DbContextOptionsBuilder dbContextBuilder, IServiceProvider serviceProvider)
        {
            var builder = new EntityMapperOptionsBuilder();

            var configType = typeof(IMapperConfiguration<>).MakeGenericType(dbContextBuilder.Options.ContextType);

            foreach (IMapperConfiguration? configService in serviceProvider.GetServices(configType))
            {
                configService?.ConfigureMapper(builder);
            }

            UseMapping(dbContextBuilder, builder.Options);

            return dbContextBuilder;
        }

        public static IServiceCollection ConfigureMapper<TDbContext>(this IServiceCollection services, Action<EntityMapperOptionsBuilder> config)
            where TDbContext : DbContext
        {
            services.AddTransient<IMapperConfiguration<TDbContext>>(sp => new DelegateMapperConfiguration<TDbContext>(config));
            return services;
        }
    }
}