using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Pagination
{
    public static class DetachedContextExtensions
    {
        public static async Task<IPaginatedResult<TProjection>> LoadPageAsync<TEntity, TProjection>(this IDetachedContext detachedContext, IPaginatedRequest<TEntity> request, Expression<Func<TEntity, TProjection>> projection)
            where TEntity : class
            where TProjection : class
        {
            IQueryable<TEntity> query = detachedContext.GetBaseQuery<TEntity>();

            IPaginatedResult<TProjection> result = new PaginatedResult<TProjection>();
            result.PageIndex = Math.Max(1, request.PageIndex);
            result.PageSize = request.PageSize;

            if (request.Filter != null)
                query = query.Where(request.Filter);

            if (request.Sorter != null)
                query = request.Sorter.Apply(query);

            if (request.RowCount == 0)
                result.RowCount = await query.CountAsync();
            else
                result.RowCount = request.RowCount;

            result.PageCount = (int)Math.Ceiling((result.RowCount / Math.Max(result.PageSize, 1.0)));

            if (result.PageSize > 1)
            {
                query = query.Skip((request.PageIndex - 1) * request.PageSize)
                             .Take(request.PageSize);
            }

            result.Items = await query.Select(projection).ToListAsync();

            return result;
        }

        public static async Task<IPaginatedResult<TEntity>> LoadPageAsync<TEntity>(this IDetachedContext detachedContext, IPaginatedRequest<TEntity> request)
            where TEntity : class
        {
            return await LoadPageAsync(detachedContext, request, e => e);
        }
    }
}
