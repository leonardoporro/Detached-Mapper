using EntityFrameworkCore.Detached.DataAnnotations;
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
        const string DETACHED_IGNORE = "DETACHED_IGNORE";

        /// <summary>
        /// Returns whether this navigation property is configured as Owned.
        /// </summary>
        /// <returns>True if the property is owned.</returns>
        public static bool IsOwned(this Navigation navigation)
        {
            var annotation = navigation.FindAnnotation(typeof(OwnedAttribute).FullName);
            return annotation != null;
        }

        /// <summary>
        /// Returns whether this navigation property is configured as Associated.
        /// </summary>
        /// <returns>True if the property is associated.</returns>
        public static bool IsAssociated(this Navigation navigation)
        {
            var annotation = navigation.FindAnnotation(typeof(AssociatedAttribute).FullName);
            return annotation != null;
        }

        /// <summary>
        /// Returns a member expression for this property.
        /// The expression should look like: entity => entity.[property.Name].
        /// </summary>
        public static Expression GetMemberExpression(this PropertyBase property)
        {
            Type entityType = property.DeclaringEntityType.ClrType;
            Type delType = typeof(Func<,>).MakeGenericType(entityType,
                                                           property.PropertyInfo.PropertyType);

            ParameterExpression param = Expression.Parameter(entityType, entityType.Name.ToLower());
            return Expression.Lambda(delType, Expression.Property(param, property.PropertyInfo), param);
        }

        /// <summary>
        /// Gets a collection value as a PrimaryKey -> Entity hash table to do lookups in O(1).
        /// </summary>
        /// <param name="navigation">The navigation property.</param>
        /// <param name="instance">The entity instance.</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetValueAsHashTable(this Navigation navigation, object instance)
        {
            ICollection collection = navigation.Getter.GetClrValue(instance) as ICollection;
            EntityType entityType = navigation.GetTargetType();
            return entityType.CreateHashTable(collection);
        }

        /// <summary>
        /// Ignores a property for the detached Update/Delete process.
        /// </summary>
        /// <param name="property">The property to ignore.</param>
        public static void DetachedIgnore(this PropertyBase property)
        {
            property.SetAnnotation(DETACHED_IGNORE, true, ConfigurationSource.DataAnnotation);
        }

        /// <summary>
        /// Gets whether a property is ignored for the detached Update/Delete process.
        /// </summary>
        /// <param name="property">The property </param>
        public static bool IsDetachedIgnore(this PropertyBase property)
        {
            return property.FindAnnotation(DETACHED_IGNORE) != null;
        }
    }
}
