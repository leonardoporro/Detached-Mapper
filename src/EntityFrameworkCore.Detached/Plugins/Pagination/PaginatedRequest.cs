using EntityFrameworkCore.Detached.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Pagination
{
    public class PaginatedRequest<TEntity> : IPaginatedRequest<TEntity>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public Expression<Func<TEntity, bool>> Filter { get; set; }

        public ISorter<TEntity> Sorter { get; set; }
    }
}
