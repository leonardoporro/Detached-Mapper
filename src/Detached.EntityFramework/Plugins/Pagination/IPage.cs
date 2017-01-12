using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Plugins.Pagination
{
    /// <summary>
    /// Represents a result of a paginated query.
    /// </summary>
    /// <typeparam name="TEntity">Clr type of the entity being queried.</typeparam>
    public interface IPage<TEntity>
    {
        /// <summary>
        /// Gets the current page index.
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the amount of pages.
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the total row count of the query.
        /// </summary>
        int RowCount { get; set; }

        /// <summary>
        /// Gets the items for the current page.
        /// </summary>
        List<TEntity> Items { get; set; }
    }
}
