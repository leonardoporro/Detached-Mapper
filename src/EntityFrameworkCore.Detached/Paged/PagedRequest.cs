using EntityFrameworkCore.Detached.DataAnnotations.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Detached.Paged
{
    public class PagedRequest<TEntity> : IPagedRequest<TEntity>
    {
        public Expression<Func<TEntity, bool>> FilterBy { get; set; }

        public string OrderBy { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
