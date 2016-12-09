using EntityFrameworkCore.Detached.Plugins.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Controllers
{
    [Route("api/[controller]")]
    public class ControllerBase<TDbContext, TEntity, TQuery> : Controller
        where TDbContext : DbContext
        where TEntity : class
        where TQuery : IQuery<TEntity>
    {
        #region Fields

        IDetachedContext<TDbContext> _detachedContext;

        #endregion

        #region Ctor.

        public ControllerBase(IDetachedContext<TDbContext> detachedContext)
        {
            _detachedContext = detachedContext;
        }

        #endregion

        [HttpGet("{id}")]
        public virtual async Task<TEntity> FindById(string id)
        {
            return await _detachedContext.Set<TEntity>().LoadAsync(id);
        }

        [HttpGet]
        public virtual async Task<IList<TEntity>> Get(TQuery query)
        {
            return await _detachedContext.Set<TEntity>().LoadAsync(q => query?.Apply(q));
        }

        [HttpGet("page/{pageIndex:int}")]
        public virtual async Task<IPage<TEntity>> GetPage(int pageIndex, int pageSize = 5, TQuery query = default(TQuery))
        {
            return await _detachedContext.Set<TEntity>().LoadPageAsync(pageIndex, pageSize, q => query?.Apply(q));
        }

        [HttpPost]
        public virtual async Task<TEntity> Save([FromBody]TEntity entity)
        {
            entity = await _detachedContext.Set<TEntity>().UpdateAsync(entity);
            await _detachedContext.SaveChangesAsync();

            return entity;
        }

        [HttpDelete]
        public virtual async Task Delete(string id)
        {
            await _detachedContext.Set<TEntity>().DeleteAsync(id);
            await _detachedContext.SaveChangesAsync();
        }
    }
}