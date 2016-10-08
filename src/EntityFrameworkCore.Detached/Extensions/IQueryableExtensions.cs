using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public static class IQueryableExtensions
    {
        static MethodInfo InternalOrderByPropertyNameMethodInfo =
            typeof(IQueryableExtensions).GetMethod(nameof(InternalOrderByPropertyName), BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Orders the given query by the specified property name (as string).
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="query">Query to order.</param>
        /// <param name="propertyName">Name of the property to order by.</param>
        /// <param name="asc">True for Ascending, False for descending.</param>
        /// <returns>The ordered queryable.</returns>
        public static IOrderedQueryable<TEntity> OrderByPropertyName<TEntity>(this IQueryable<TEntity> query, string propertyName, bool asc = true)
        {
            Type clrType = typeof(TEntity);
            PropertyInfo propInfo = clrType.GetPropertyByNameCaseInsensitive(propertyName);
            MethodInfo methodInfo = InternalOrderByPropertyNameMethodInfo.MakeGenericMethod(clrType, propInfo.PropertyType);
            return (IOrderedQueryable<TEntity>)methodInfo.Invoke(null, new object[] { query, clrType, propInfo, asc });
        }

        static IOrderedQueryable<TEntity> InternalOrderByPropertyName<TEntity, TValue>(IQueryable<TEntity> query, Type clrType, PropertyInfo propInfo, bool asc)
        {
            ParameterExpression param = Expression.Parameter(clrType);
            Expression body = Expression.Property(param, propInfo);

            var lambda = Expression.Lambda<Func<TEntity, TValue>>(body, param);

            return asc ? query.OrderBy(lambda) : query.OrderByDescending(lambda);
        }
    }
}
