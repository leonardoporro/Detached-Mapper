using EntityFrameworkCore.Detached.Contracts;
using EntityFrameworkCore.Detached.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EntityFrameworkCore.Detached
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Configures the context to use EF Core Detached.
        /// Adds the Convention Set Builder needed to process the [Owned] and [Attached] attributes.
        /// </summary>
        public static DbContextOptionsBuilder UseDetached(this DbContextOptionsBuilder builder)
        {
            builder.ReplaceService<ICoreConventionSetBuilder, DetachedCoreConventionSetBuilder>();
            return builder;
        }

        /// <summary>
        /// Configures the service collection to use EF Core Detached.
        /// Adds the Convention Set Builder needed to process the [Owned] and [Attached] attributes.
        /// Adds [ManyToMany] patch.
        /// Adds [CreatedBy] [CreatedDate] [ModifiedBy] and [ModifiedDate] handling.
        /// </summary>
        public static IServiceCollection AddEntityFrameworkDetached(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICoreConventionSetBuilder, DetachedCoreConventionSetBuilder>();
            serviceCollection.AddTransient(typeof(IDetachedContext<>), typeof(DetachedContext<>));

            return serviceCollection;
        }

        /// <summary>
        /// Adds a IDetachedSessionInfoProvider to get the user name and date for audit purposes.
        /// </summary>
        public static IServiceCollection AddSessionInfoProvider<TProvider>(this IServiceCollection serviceCollection)
            where TProvider : class, IDetachedSessionInfoProvider
        {
            serviceCollection.AddScoped<IDetachedSessionInfoProvider, TProvider>();
            return serviceCollection;
        }

        /// <summary>
        /// Adds a IDetachedSessionInfoProvider to get the user name and date for audit purposes.
        /// </summary>
        public static IServiceCollection AddSessionInfoProvider(this IServiceCollection serviceCollection, Func<string> getCurrentUser)
        {
            serviceCollection.AddSingleton<IDetachedSessionInfoProvider>(new DelegateSessionInfoProvider(getCurrentUser));
            return serviceCollection;
        }
    }
}
