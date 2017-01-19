using Detached.EntityFramework.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Services
{
    public class DetachedQuery<TEntity> : IDetachedQuery<TEntity>
        where TEntity : class
    {
        public string OrderBy { get; set; }

        public string SearchText { get; set; }

        public virtual IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            query = ApplyCommon(query);
            query = ApplyCustom(query);
            return query;
        }

        protected virtual IQueryable<TEntity> ApplyCommon(IQueryable<TEntity> query)
        {
            query = Sorter<TEntity>.FromString(OrderBy).Apply(query);
            return query;
        }

        protected virtual IQueryable<TEntity> ApplyCustom(IQueryable<TEntity> query)
        {
            return query;
        }
    }
}
