using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Pagination
{
    public interface IPaginatedResult<TEntity>
    {
        int RowCount { get; set; }

        int PageSize { get; set; }

        int PageCount { get; set; }

        int PageIndex { get; set; }

        List<TEntity> Items { get; set; }
    }
}
