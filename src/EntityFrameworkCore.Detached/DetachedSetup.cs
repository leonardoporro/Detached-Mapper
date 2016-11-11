using EntityFrameworkCore.Detached.Conventions;
using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using EntityFrameworkCore.Detached.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public static class DetachedSetup
    {
        public static IServiceCollection AddDetachedEntityFramework(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICoreConventionSetBuilder, CustomCoreConventionSetBuilder>()
                             .AddScoped<IDetachedServices, DetachedServices>()
                             .AddScoped<IEventManager, EventManager>()
                             .AddScoped<IPluginManager, PluginManager>()
                             .AddScoped<IKeyServicesFactory, KeyServicesFactory>()
                             .AddScoped<IEntryFinder, EntryFinder>()
                             .AddScoped<ILoadServices, LoadServices>()
                             .AddScoped<IUpdateServices, UpdateServices>()
                             .AddScoped<DbContext>(sp => sp.GetRequiredService<IDetachedServices>().DetachedContext.DbContext);

            return serviceCollection;
        }

    public static IServiceCollection AddConventionBuilder<TConventionBuilder>(this IServiceCollection serviceCollection)
        where TConventionBuilder : class, ICustomConventionBuilder
    {
        serviceCollection.AddScoped<ICustomConventionBuilder, TConventionBuilder>();
        return serviceCollection;
    }

    public static DbContextOptionsBuilder UseDetached(this DbContextOptionsBuilder builder, Action<DetachedOptionsExtension> options = null)
    {
        DetachedOptionsExtension detachedOptions = new DetachedOptionsExtension();
        options?.Invoke(detachedOptions);
        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(detachedOptions);
        return builder;
    }

    public static DetachedOptionsExtension AddSingleton<TService>(this DetachedOptionsExtension options, TService instance)
        where TService : class
    {
        options.DetachedServices.AddSingleton(instance);
        return options;
    }

    public static DetachedOptionsExtension AddScoped<TService, TImplementation>(this DetachedOptionsExtension options)
       where TService : class
       where TImplementation : class, TService
    {
        options.DetachedServices.AddScoped<TService, TImplementation>();
        return options;
    }
}
}
