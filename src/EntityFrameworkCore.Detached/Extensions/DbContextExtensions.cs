using EntityFrameworkCore.Detached.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        /// </summary>
        public static IServiceCollection AddEntityFrameworkDetached(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICoreConventionSetBuilder, DetachedCoreConventionSetBuilder>();
            return serviceCollection;
        }

        //public static ReferenceNavigationBuilder<TEntity, TProperty> OwnsOne<TEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, IEnumerable<TProperty>>> navigationExpression)
        //    where TEntity : class
        //{
        //    builder.HasOne(navigationExpression);

        //    string name = ((MemberExpression)navigationExpression.Body).Member.Name;
        //    Navigation nav = builder.Metadata.GetNavigations().Where(e => e.Name == name).FirstOrDefault() as Navigation;
        //    nav.AddAnnotation("Owned", true);

        //    return builder;
        //}
    }
}
