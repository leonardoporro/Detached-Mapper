using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Tools
{
    public class SortEntry<TEntity, TResult> : ISortEntry<TEntity>
    {
        public SortEntry()
        {

        }

        public SortEntry(PropertyInfo propInfo, bool ascending)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            Expression body = Expression.Property(param, propInfo);
            SortMemberExpression = Expression.Lambda<Func<TEntity, TResult>>(body, param);

            Ascending = ascending;
        }

        public Expression<Func<TEntity, TResult>> SortMemberExpression { get; set; }

        public bool Ascending { get; set; }

        public IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query)
        {
            if (Ascending)
                return query.OrderBy(SortMemberExpression);
            else
                return query.OrderByDescending(SortMemberExpression);
        }

        public IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query)
        {
            if (Ascending)
                return query.ThenBy(SortMemberExpression);
            else
                return query.ThenBy(SortMemberExpression);
        }
    }
}
