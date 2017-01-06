using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Controllers
{
    public interface IQuery<TEntity>
    {
        string OrderBy { get; set; }

        string SearchText { get; set; }

        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}
