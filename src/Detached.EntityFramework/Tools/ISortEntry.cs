using System.Linq;

namespace Detached.EntityFramework.Tools
{
    /// <summary>
    /// Encapsulates a member to order by.
    /// </summary>
    /// <typeparam name="TEntity">Clr type of the entity containing the member to order by.</typeparam>
    public interface ISortEntry<TEntity>
    {
        /// <summary>
        /// Applies an OrderBy clause to the given query.
        /// </summary>
        /// <param name="query">A query to order by.</param>
        /// <returns>The query, ordered by the current member.</returns>
        IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query);

        /// <summary>
        /// Applies a ThenBy clause to the given query.
        /// </summary>
        /// <param name="query">A query to order by.</param>
        /// <returns>The query, ordered by the current member.</returns>
        IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query);
    }
}
