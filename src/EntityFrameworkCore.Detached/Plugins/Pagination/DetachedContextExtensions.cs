using EntityFrameworkCore.Detached.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Pagination
{
    /// <summary>
    /// Provides paged query extensions to a detached context.
    /// </summary>
    public static class DetachedContextExtensions
    {
        /// <summary>
        /// Loads a page of the specified entity, and projects it to the given type.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the entity.</typeparam>
        /// <typeparam name="TProjection">Clr type of the result object.</typeparam>
        /// <param name="detachedSet">The detached context.</param>
        /// <param name="request">The request to load.</param>
        /// <param name="projection">The projection function.</param>
        /// <returns>A projected and paged result.</returns>
        public static async Task<IPage<TProjection>> LoadPageAsync<TEntity, TProjection>(this IDetachedSet<TEntity> detachedSet, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IQueryable<TProjection>> configureQuery = null)
            where TEntity : class
            where TProjection : class
        {
            IPage<TProjection> result = new Page<TProjection>();
            result.PageIndex = Math.Max(1, pageIndex);
            result.PageSize = pageSize;

            IQueryable<TEntity> query = detachedSet.GetBaseQuery();
            IQueryable<TProjection> finalQuery = configureQuery?.Invoke(query);

            result.RowCount = await finalQuery.CountAsync();
            result.PageCount = (int)Math.Ceiling((result.RowCount / Math.Max(result.PageSize, 1.0)));

            if (result.PageSize > 1)
            {
                finalQuery = finalQuery.Skip((pageIndex - 1) * pageSize)
                                       .Take(pageSize);
            }

            result.Items = await finalQuery.ToListAsync();

            return result;
        }
    }
}