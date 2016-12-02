using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tools
{
    /// <summary>
    /// Builds a sort expression to be used later as a paramenter in a service or query.
    /// </summary>
    /// <typeparam name="TEntity">Clr type of the entity being sorted.</typeparam>
    public interface ISorterBase<TEntity>
    {
        /// <summary>
        /// Applies this order to the given query.
        /// </summary>
        /// <param name="query">Query to order by this sorter.</param>
        /// <returns>The query, ordered by this sorter.</returns>
        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}
