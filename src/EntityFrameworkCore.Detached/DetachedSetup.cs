using EntityFrameworkCore.Detached.Conventions;
using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using EntityFrameworkCore.Detached.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EntityFrameworkCore.Detached
{
    /// <summary>
    /// Provides extension methods to configure EntityFramework.Detached.
    /// </summary>
    public static class DetachedSetup
    {
        /// <summary>
        /// Adds EntityFramework.Detached services to the given ServiceCollection.
        /// </summary>
        /// <param name="serviceCollection">The ServiceCollection that will be used as internal service.
        /// provider.</param>
        /// <returns>The same ServiceCollection for chained call purposes.</returns>
        public static IServiceCollection AddDetachedEntityFramework(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICoreConventionSetBuilder, CustomCoreConventionSetBuilder>()
                             .AddScoped<IDbContextServices, DetachedContextServices>()
                             .AddScoped<IDetachedContextServices>(s => (IDetachedContextServices)s.GetService<IDbContextServices>())
                             .AddScoped<IEventManager, EventManager>()
                             .AddScoped<IPluginManager, PluginManager>()
                             .AddScoped<IEntityServicesFactory, EntityServicesFactory>()
                             .AddScoped(typeof(IDetachedSet<>), typeof(DetachedSet<>))
                             .AddScoped<IEntryFinder, EntryFinder>()
                             .AddScoped<ILoadServices, LoadServices>()
                             .AddScoped<IUpdateServices, UpdateServices>()
                             .AddScoped(sp => sp.GetRequiredService<IDetachedContextServices>().DetachedContext.DbContext)
                             .AddScoped(sp => sp.GetRequiredService<IDetachedContextServices>().DetachedContext)
                             .AddScoped(sp => sp.GetRequiredService<IDetachedContextServices>().DetachedOptions);

            return serviceCollection;
        }

        /// <summary>
        /// Registers an ICustomConvetnionBuilder instance that allows to add/remove conventions before the
        /// model is created.
        /// </summary>
        /// <typeparam name="TConventionBuilder">Type of the ICustomConvetnionBuilder implementation.</typeparam>
        /// <param name="serviceCollection">he ServiceCollection that will be used as internal service.</param>
        /// <returns>The same ServiceCollection for chained call purposes.</returns>
        public static IServiceCollection AddConventionBuilder<TConventionBuilder>(this IServiceCollection serviceCollection)
            where TConventionBuilder : class, ICustomConventionBuilder
        {
            serviceCollection.AddScoped<ICustomConventionBuilder, TConventionBuilder>();
            return serviceCollection;
        }

        /// <summary>
        /// Configures the DbContext to use EntityFramework.Detached.
        /// </summary>
        /// <param name="builder">The DbContextOptionsBuilder instance.</param>
        /// <param name="options">A delegate that configures the Detached Options.</param>
        /// <returns>The same DbCotnextOptionsBuilder for chaining calls.</returns>
        public static DbContextOptionsBuilder UseDetached(this DbContextOptionsBuilder builder, Action<DetachedOptionsExtension> options = null)
        {
            DetachedOptionsExtension detachedOptions = new DetachedOptionsExtension();
            options?.Invoke(detachedOptions);
            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(detachedOptions);
            return builder;
        }

        /// <summary>
        /// Adds an internal service with a singleton life time.
        /// </summary>
        /// <typeparam name="TService">Clr Type of the Contract.</typeparam>
        /// <param name="options">Detached Options instance.</param>
        /// <param name="instance">Service implementation instance.</param>
        /// <returns>The same DetachedOptionsExtensions, for chaining calls.</returns>
        public static DetachedOptionsExtension AddSingleton<TService>(this DetachedOptionsExtension options, TService instance)
            where TService : class
        {
            options.DetachedServices.AddSingleton(instance);
            return options;
        }

        /// <summary>
        /// Adds an internal service with a scoped life time.
        /// </summary>
        /// <typeparam name="TService">Clr type of the contract.</typeparam>
        /// <typeparam name="TImplementation">Clr type of the implementation.</typeparam>
        /// <param name="options">Detached Options instance.</param>
        /// <returns>The same DetachedOptionsExtensions, for chaining calls.</returns>
        public static DetachedOptionsExtension AddScoped<TService, TImplementation>(this DetachedOptionsExtension options)
           where TService : class
           where TImplementation : class, TService
        {
            options.DetachedServices.AddScoped<TService, TImplementation>();
            return options;
        }
    }
}