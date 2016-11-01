using System;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Detached.Plugins.Pagination
{
    public interface IPaginatedQuery<TEntity>
    {
        int PageSize { get; set; }

        int PageIndex { get; set; }

        Expression<Func<TEntity, bool>> FilterBy { get; set; }

        string OrderBy { get; set; }
    }
}
