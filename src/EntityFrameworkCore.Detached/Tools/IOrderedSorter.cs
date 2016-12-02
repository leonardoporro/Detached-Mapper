using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tools
{
    /// <summary>
    /// Builds a sort expression to be used later as a paramenter in a service or query.
    /// </summary>
    /// <typeparam name="TEntity">Clr type of the entity being sorted.</typeparam>
    public interface IOrderedSorter<TEntity> : ISorterBase<TEntity>
    {
        /// <summary>
        /// Orders by the specified property of the given entity.
        /// </summary>
        /// <typeparam name="TKey">Clr type of the key to order by.</typeparam>
        /// <param name="keySelector"></param>
        /// <returns>The ISorter instance, for chaining calls.</returns>
        IOrderedSorter<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        /// <summary>
        /// Orders by the specified property of the given entity in descending order.
        /// </summary>
        /// <typeparam name="TKey">Clr type of the key to order by.</typeparam>
        /// <param name="keySelector"></param>
        /// <returns>The ISorter instance, for chaining calls.</returns>
        IOrderedSorter<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);
    }
}
