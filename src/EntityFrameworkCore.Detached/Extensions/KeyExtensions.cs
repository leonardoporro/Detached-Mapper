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
    public static class KeyExtensions
    {
        /// <summary>
        /// Compares two in-memory entities by its key.
        /// </summary>
        /// <param name="key">Key instance of the entities to compare.</param>
        /// <param name="entityA">Entity to compare.</param>
        /// <param name="entityB">Entity to compare.</param>
        /// <returns></returns>
        public static bool Equal(this IKey key, object entityA, object entityB)
        {
            bool equal = entityA != null && entityB != null;
            if (equal)
            {
                foreach (Property property in key.Properties)
                {
                    object aValue = property.Getter.GetClrValue(entityA);
                    object bValue = property.Getter.GetClrValue(entityB);
                    if (!object.Equals(aValue, bValue))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }

        /// <summary>
        /// Gets the key values for the given entity.
        /// </summary>
        /// <param name="key">The entity key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>An array of objects containing key values.</returns>
        public static object[] GetKeyValues(this IKey key, object entity)
        {
            return key.Properties
                      .Select(p => p.GetGetter().GetClrValue(entity))
                      .ToArray();
        }

        /// <summary>
        /// Gets a string reperesentation of the key values for the given entity.
        /// </summary>
        /// <param name="key">The entity key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>An array of objects containing key values.</returns>
        public static string GetKey(this IKey key, object entity)
        {
            return string.Join("&", key.Properties.Select(p => p.Name + "=" + p.GetGetter().GetClrValue(entity)));
        }

        /// <summary>
        /// Creates a Key-Entity dictionary for the given entity enumeration.
        /// </summary>
        /// <param name="key">The entity key-</param>
        /// <param name="entities">Entity list to fill the table.</param>
        /// <returns></returns>
        public static Dictionary<string, object> CreateKeyEntityTable(this IKey key, IEnumerable entities)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (entities != null)
            {
                foreach (object entity in entities)
                    result.Add(key.GetKey(entity), entity);
            }
            return result;
        }

        /// <summary>
        /// Returns an expression that can be used as a query filter (where) to find
        /// an entity by its key.
        /// </summary>
        /// <typeparam name="TEntity">Entity Clr Type.</typeparam>
        /// <param name="entityType">The entity metadata.</param>
        /// <param name="keyValues">The entity key values.</param>
        /// <returns>An expression to filter by.</returns>
        public static Expression<Func<TEntity, bool>> CreateFindExpression<TEntity>(this IKey key, object[] keyValues)
        {
            if (keyValues == null || keyValues.Any(kv => kv == null))
                throw new ArgumentException("Key values cannot be null.", nameof(keyValues));

            if (key.Properties.Count != keyValues.Length)
                throw new ArgumentException($"Key values count mismatch, expected {string.Join(",", key.Properties.Select(p => p.Name))}");

            ParameterExpression param = Expression.Parameter(key.DeclaringEntityType.ClrType);
            Func<int, Expression> buildCompare = i =>
            {
                object keyValue = keyValues[i];
                IProperty keyProperty = key.Properties[i];
                if (keyValue.GetType() != keyProperty.ClrType)
                {
                    keyValue = Convert.ChangeType(keyValue, keyProperty.ClrType);
                }

                return Expression.Equal(Expression.Property(param, keyProperty.PropertyInfo),
                                        Expression.Constant(keyValue));
            };

            Expression findExpr = buildCompare(0);
            for (int i = 1; i < key.Properties.Count; i++)
            {
                findExpr = Expression.AndAlso(findExpr, buildCompare(i));
            }

            return Expression.Lambda<Func<TEntity, bool>>(findExpr, param);
        }
    }
}
