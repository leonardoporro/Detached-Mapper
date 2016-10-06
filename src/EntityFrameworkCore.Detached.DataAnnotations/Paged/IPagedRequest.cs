using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.DataAnnotations.Paged
{
    public interface IPagedRequest<TEntity>
    {
        int PageSize { get; set; }

        int PageIndex { get; set; }

        Expression<Func<TEntity, bool>> FilterBy { get; set; }

        string OrderBy { get; set; }
    }
}
