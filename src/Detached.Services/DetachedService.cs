using Detached.EntityFramework;
using Detached.EntityFramework.Plugins.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Services
{
    public class DetachedService<TDbContext, TEntity, TQuery> 
        where TDbContext : DbContext
        where TEntity : class
        where TQuery : IDetachedQuery<TEntity>
    {
        IDetachedContext<TDbContext> _detachedContext;

        public DetachedService(IDetachedContext<TDbContext> detachedContext)
        {
            _detachedContext = detachedContext;
        }

        public virtual async Task<TEntity> FindById(object key)
        {
            return await _detachedContext.Set<TEntity>().LoadAsync(key);
        }

        public virtual async Task<IList<TEntity>> Get(TQuery query = default(TQuery))
        {
            return await _detachedContext.Set<TEntity>().LoadAsync(q => query?.Apply(q));
        }

        public virtual async Task<IPage<TEntity>> GetPage(int pageIndex, int pageSize = 5, bool noCount = false, TQuery query = default(TQuery))
        {
            return await _detachedContext.Set<TEntity>().LoadPageAsync(pageIndex, pageSize, q => query?.Apply(q));
        }

        public virtual async Task<TEntity> Save(TEntity entity)
        {
            entity = await _detachedContext.Set<TEntity>().UpdateAsync(entity);
            await _detachedContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task Delete(object key)
        {
            await _detachedContext.Set<TEntity>().DeleteAsync(key);
            await _detachedContext.SaveChangesAsync();
        }
    }
}
