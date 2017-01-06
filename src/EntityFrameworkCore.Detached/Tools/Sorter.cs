using EntityFrameworkCore.Detached.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tools
{
    /// <summary>
    /// Builds a sort expression to be used later as a paramenter in a service or query.
    /// </summary>
    /// <typeparam name="TEntity">Clr type of the entity being sorted.</typeparam>
    public class Sorter<TEntity> : ISorter<TEntity>, IOrderedSorter<TEntity>
    {
        List<ISortEntry<TEntity>> entries = new List<ISortEntry<TEntity>>();

        public IOrderedSorter<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { SortMemberExpression = keySelector, Ascending = true });
            return this;
        }

        public IOrderedSorter<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { SortMemberExpression = keySelector, Ascending = false });
            return this;
        }

        public IOrderedSorter<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { SortMemberExpression = keySelector, Ascending = true });
            return this;
        }

        public IOrderedSorter<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { SortMemberExpression = keySelector, Ascending = false });
            return this;
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            if (entries.Count > 0)
            {
                IOrderedQueryable<TEntity> orderedQuery = entries[0].ApplyOrderBy(query);
                for (int i = 1; i < entries.Count; i++)
                {
                    orderedQuery = entries[1].ApplyThenBy(orderedQuery);
                }
                return orderedQuery;
            }
            return query;
        }

        /// <summary>
        /// Creates a new sorter instance from the given string.
        /// Accepted formats are:
        /// +Column
        /// -Column
        /// Column ASC
        /// Column DESC
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IOrderedSorter<TEntity> FromString(string orderBy)
        {
            Sorter<TEntity> sorter = new Tools.Sorter<TEntity>();

            if (!string.IsNullOrEmpty(orderBy))
            {
                Type entityType = typeof(TEntity);
                Dictionary<string, PropertyInfo> entityProps = entityType.GetRuntimeProperties().ToDictionary(r => r.Name.ToLower(), r => r);

                string[] orderByProps = orderBy.Split(';', ',');
                foreach (string orderByProp in orderByProps)
                {
                    string propertyName;
                    string order;
                    // accept +ColumnName and -ColumnName
                    if (orderByProp.StartsWith("+") || orderByProp.StartsWith("-"))
                    {
                        propertyName = orderByProp.Substring(1);
                        order = orderByProp.StartsWith("+") ? "asc" : "desc";
                    }
                    else //accept ColumnName ASC and ColumnName DESC
                    {
                        string[] orderByPart = orderByProp.Split(' ');

                        propertyName = orderByPart[0];
                        order = orderByPart.Length > 1 ? orderByPart[1].Trim().ToLower() : "asc";
                    }

                    PropertyInfo propInfo;
                    if (!entityProps.TryGetValue(propertyName, out propInfo))
                        throw new ArgumentException($"Error applying OrderBy. Property {propertyName} was not found in object {entityType.Name}.");

                    Type entryType = typeof(SortEntry<,>).MakeGenericType(entityType, propInfo.PropertyType);
                    ISortEntry<TEntity> sortEntry = (ISortEntry<TEntity>)Activator.CreateInstance(entryType, propInfo, order == "asc");
                    sorter.entries.Add(sortEntry);
                }
            }

            return sorter;
        }
    }
}