using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.DataAnnotations
{
    public interface IPagedResult<TEntity>
        where TEntity : class
    {
        int RowCount { get; set; }

        int PageCount { get; set; }

        int PageIndex { get; set; }

        int PageSize { get; set; }

        List<TEntity> Items { get; set; }
    } 
}
