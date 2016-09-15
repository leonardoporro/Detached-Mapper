using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public static class NavigationExtensions
    {
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
    }
}
