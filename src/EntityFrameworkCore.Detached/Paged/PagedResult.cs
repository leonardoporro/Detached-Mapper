using EntityFrameworkCore.Detached.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Paged
{
    public class PagedResult<TEntity> : IPagedResult<TEntity>
        where TEntity : class
    {
        public List<TEntity> Items { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }
    }
}
