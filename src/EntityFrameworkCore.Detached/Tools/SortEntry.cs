using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tools
{
    public class SortEntry<TEntity, TResult> : ISortEntry<TEntity>
    {
        public Expression<Func<TEntity, TResult>> Expression { get; set; }

        public bool Ascending { get; set; }

        public IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query)
        {
            if (Ascending)
                return query.OrderBy(Expression);
            else
                return query.OrderByDescending(Expression);
        }

        public IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query)
        {
            if (Ascending)
                return query.ThenBy(Expression);
            else
                return query.ThenBy(Expression);
        }
    }
}
