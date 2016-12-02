using EntityFrameworkCore.Detached.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tools
{
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

        public static IOrderedSorter<TEntity> FromString(string orderBy)
        {
            Sorter<TEntity> sorter = new Tools.Sorter<TEntity>();

            Type entityType = typeof(TEntity);

            Dictionary<string, PropertyInfo> entityProps = entityType.GetRuntimeProperties().ToDictionary(r => r.Name.ToLower(), r => r);

            string[] orderByProps = orderBy.Split(';', ',');
            foreach (string orderByProp in orderByProps)
            {
                string[] orderByPart = orderByProp.Split(' ');

                string propertyName = orderByPart[0];
                string order = orderByPart.Length > 1 ? orderByPart[1].Trim().ToLower() : "asc";

                PropertyInfo propInfo;
                if (!entityProps.TryGetValue(propertyName, out propInfo))
                    throw new ArgumentException($"Error applying OrderBy. Property {propertyName} was not found in object {entityType.Name}.");

                Type entryType = typeof(SortEntry<,>).MakeGenericType(entityType, propInfo.PropertyType);
                ISortEntry<TEntity> sortEntry = (ISortEntry<TEntity>)Activator.CreateInstance(entryType, propInfo, order == "asc");

                sorter.entries.Add(sortEntry);
            }

            return sorter;
        }
    }
}
