using Detached.Model.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Model.EntityFramework
{
    public static class Package
    {
        public static DbContextOptionsBuilder UseModel(this DbContextOptionsBuilder dbContextBuilder, IServiceProvider serviceProvider)
        {
            var builder = (IDbContextOptionsBuilderInfrastructure)dbContextBuilder;
            var dbContextType = dbContextBuilder.Options.ContextType;
            var setupType = typeof(IModelConfiguration<>).MakeGenericType(dbContextType);
            var setups = serviceProvider.GetServices(setupType);
            var customizerType = typeof(ModelCustomizer<>).MakeGenericType(dbContextType);
            var customizer = (IModelCustomizer)Activator.CreateInstance(customizerType, setups)!;

            builder.AddOrUpdateExtension(new ModelDbContextOptionsExtension(dbContextType, customizer));

            return dbContextBuilder;
        }

        public static IServiceCollection ConfigureModel<TDbContext>(this IServiceCollection services, Action<ModelBuilder, TDbContext> configure)
            where TDbContext : DbContext
        {
            services.AddTransient(sp => new DelegateModelConfiguration<TDbContext>(configure));

            return services;
        }
    }
}
