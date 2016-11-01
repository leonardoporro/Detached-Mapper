using EntityFrameworkCore.Detached.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
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
            serviceCollection.AddScoped<ICoreConventionSetBuilder, CustomCoreConventionSetBuilder>();
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
