using EntityFrameworkCore.Detached.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tools
{
    public class Sorter<TEntity> : ISorter<TEntity>
    {
        List<ISortEntry<TEntity>> entries = new List<ISortEntry<TEntity>>();

        public ISorter<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { Expression = keySelector, Ascending = true });
            return this;
        }

        public ISorter<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { Expression = keySelector,  Ascending = false });
            return this;
        }

        public ISorter<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { Expression = keySelector,  Ascending = true });
            return this;
        }

        public ISorter<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            entries.Add(new SortEntry<TEntity, TKey> { Expression = keySelector, Ascending = false });
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
    }
}
