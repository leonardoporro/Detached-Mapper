using EntityFrameworkCore.Detached.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Pagination
{
    public interface IPaginatedRequest<TEntity>
    {
        int PageSize { get; }

        int PageIndex { get; }

        int RowCount { get; }

        Expression<Func<TEntity, bool>> Filter { get; set; }

        ISorter<TEntity> Sorter { get; }
    }
}
