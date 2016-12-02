using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    /// <summary>
    /// Provides extra features for properties.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Returns whether this navigation property is configured as Owned.
        /// </summary>
        /// <returns>True if the property is owned.</returns>
        public static bool IsOwned(this INavigation navigation)
        {
            var annotation = navigation.FindAnnotation(typeof(OwnedAttribute).FullName);
            return annotation != null;
        }

        /// <summary>
        /// Set this navigation property as Owned.
        /// </summary>
        public static void Owned(this Navigation navigation, ConfigurationSource source)
        {
            var annotation = navigation.SetAnnotation(typeof(OwnedAttribute).FullName, true, source);
        }

        /// <summary>
        /// Returns whether this navigation property is configured as Associated.
        /// </summary>
        /// <returns>True if the property is associated.</returns>
        public static bool IsAssociated(this INavigation navigation)
        {
            var annotation = navigation.FindAnnotation(typeof(AssociatedAttribute).FullName);
            return annotation != null;
        }

        /// <summary>
        /// Returns whether this navigation property is configured as Owned.
        /// </summary>
        public static void Associated(this Navigation navigation, ConfigurationSource source)
        {
            var annotation = navigation.SetAnnotation(typeof(AssociatedAttribute).FullName, true, source);
        }

        /// <summary>
        /// Gets whether a property is ignored for the detached Update/Delete process.
        /// </summary>
        /// <param name="property">The property </param>
        public static bool IsIgnored(this IPropertyBase property)
        {
            return property.FindAnnotation(typeof(IgnoredAtrribute).FullName) != null;
        }

        /// <summary>
        /// Ignores a property for the detached Update/Delete process.
        /// </summary>
        /// <param name="property">The property to ignore.</param>
        public static void DetachedIgnore(this PropertyBase property)
        {
            property.SetAnnotation(typeof(IgnoredAtrribute).FullName, true, ConfigurationSource.DataAnnotation);
        }

        /// <summary>
        /// Returns a member expression for this property.
        /// The expression should look like: entity => entity.[property.Name].
        /// </summary>
        public static Expression AsExpression(this IPropertyBase property)
        {
            Type entityType = property.DeclaringType.ClrType;
            Type delType = typeof(Func<,>).MakeGenericType(entityType,
                                                           property.PropertyInfo.PropertyType);

            ParameterExpression param = Expression.Parameter(entityType, entityType.Name.ToLower());
            return Expression.Lambda(delType, Expression.Property(param, property.PropertyInfo), param);
        }
    }
}