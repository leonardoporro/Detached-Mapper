using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tools
{
    public interface ISorter<TEntity>
    {
        ISorter<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        ISorter<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        ISorter<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        ISorter<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}
