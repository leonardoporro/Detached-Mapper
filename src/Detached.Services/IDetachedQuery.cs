using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Services
{
    public interface IDetachedQuery<TEntity> where TEntity : class
    {
        string OrderBy { get; set; }

        string SearchText { get; set; }

        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}
