using Detached.DataAnnotations;
using Detached.EntityFramework.Conventions;
using Detached.EntityFramework.Events;
using Detached.EntityFramework.Plugins;
using Detached.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Detached.EntityFramework
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

        /// <summary>
        /// (Detached) Configures a relationship where this entity type owns (see [Owned]) instances of the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">Clr Type of the entity.</typeparam>
        /// <typeparam name="TRelatedEntity">Clr type of the related entity.</typeparam>
        /// <param name="builder">The entity builder</param>
        /// <param name="navigationExpression">A property selector</param>
        /// <returns>A pre-configured collection builder</returns>
        public static CollectionNavigationBuilder<TEntity, TRelatedEntity> OwnsMany<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression = null)
            where TEntity : class
            where TRelatedEntity : class
        {
            CollectionNavigationBuilder<TEntity, TRelatedEntity> navigationBuilder = builder.HasMany(navigationExpression);

            RunAttributeConvention(navigationBuilder.GetInfrastructure(), navigationExpression, new OwnedNavigationAttributeConvention());

            return navigationBuilder;
        }

        /// <summary>
        /// (Detached) Configures a relationship where this entity type owns (see [Owned]) a single instance of the given entity.
        /// </summary>
        /// <typeparam name="TEntity">Clr Type of the entity.</typeparam>
        /// <typeparam name="TRelatedEntity">Clr type of the related entity.</typeparam>
        /// <param name="builder">The entity builder</param>
        /// <param name="navigationExpression">A property selector</param>
        /// <returns>A pre-configured reference builder</returns>
        public static ReferenceNavigationBuilder<TEntity, TRelatedEntity> OwnsOne<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TRelatedEntity>> navigationExpression = null)
            where TEntity : class
            where TRelatedEntity : class
        {
            ReferenceNavigationBuilder<TEntity, TRelatedEntity> navigationBuilder = builder.HasOne(navigationExpression);

            RunAttributeConvention(navigationBuilder.GetInfrastructure(), navigationExpression, new OwnedNavigationAttributeConvention());

            return navigationBuilder;
        }

        /// <summary>
        /// (Detached) Configures a relationship where this entity type has (see [Associated]) instances of the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">Clr Type of the entity.</typeparam>
        /// <typeparam name="TRelatedEntity">Clr type of the related entity.</typeparam>
        /// <param name="builder">The entity builder</param>
        /// <param name="navigationExpression">A property selector</param>
        /// <returns>A pre-configured collection builder</returns>
        public static CollectionNavigationBuilder<TEntity, TRelatedEntity> RefersMany<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression = null)
            where TEntity : class
            where TRelatedEntity : class
        {
            CollectionNavigationBuilder<TEntity, TRelatedEntity> navigationBuilder = builder.HasMany(navigationExpression);
            InternalRelationshipBuilder internalBuilder = navigationBuilder.GetInfrastructure();

            internalBuilder.Metadata.SetAnnotation(typeof(AssociatedAttribute).FullName, new AssociatedAttribute(), ConfigurationSource.Explicit);

            return navigationBuilder;
        }

        /// <summary>
        /// (Detached) Configures a relationship where this entity type has (see [Associated]) a single instance of the given entity.
        /// </summary>
        /// <typeparam name="TEntity">Clr Type of the entity.</typeparam>
        /// <typeparam name="TRelatedEntity">Clr type of the related entity.</typeparam>
        /// <param name="builder">The entity builder</param>
        /// <param name="navigationExpression">A property selector</param>
        /// <returns>A pre-configured reference builder</returns>
        public static ReferenceNavigationBuilder<TEntity, TRelatedEntity> RefersOne<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TRelatedEntity>> navigationExpression = null)
            where TEntity : class
            where TRelatedEntity : class
        {
            ReferenceNavigationBuilder<TEntity, TRelatedEntity> navigationBuilder = builder.HasOne(navigationExpression);
            RunAttributeConvention(navigationBuilder.GetInfrastructure(), navigationExpression, new AssociatedNavigationAttributeConvention());

            return navigationBuilder;
        }

        /// <summary>
        /// Adds the given attribute convention to the specified relationship builder. i.e.: Owned, Associated.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the entity</typeparam>
        /// <typeparam name="TRelatedEntity">Clr type of the association</typeparam>
        /// <typeparam name="TAttribute">DataAnnotation type</typeparam>
        /// <param name="builder">The relationship builder</param>
        /// <param name="navigationExpression">The property selector</param>
        /// <param name="convention">The convention to apply</param>
        static void RunAttributeConvention<TEntity, TRelatedEntity, TAttribute>(InternalRelationshipBuilder builder, Expression<Func<TEntity, TRelatedEntity>> navigationExpression, NavigationAttributeNavigationConvention<TAttribute> convention)
            where TAttribute : Attribute, new()
        {
            EntityType entityType = builder.ModelBuilder.Metadata.FindEntityType(typeof(TEntity));
            PropertyInfo propInfo = ((MemberExpression)navigationExpression.Body).Member as PropertyInfo;
            Navigation navigation = entityType.FindNavigation(propInfo);

            convention.Apply(builder, navigation, new TAttribute());
        }
    }
}