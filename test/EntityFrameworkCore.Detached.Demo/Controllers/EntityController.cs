using EntityFrameworkCore.Detached.Plugins.Pagination;
using EntityFrameworkCore.Detached.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Controllers
{
    [Route("api/[controller]")]
    public class EntityController<TDbContext, TEntity> : Controller
        where TDbContext : DbContext
        where TEntity : class
    {
        IDetachedContext<TDbContext> _detachedContext;

        public EntityController(IDetachedContext<TDbContext> detachedContext)
        {
            _detachedContext = detachedContext;
        }

        [HttpGet]
        public virtual async Task<List<TEntity>> GetAll()
        {
            return await _detachedContext.Set<TEntity>().LoadAsync(e => e);
        }

        [HttpGet("page/{pageIndex:int}")]
        public virtual async Task<IPagedData<TEntity>> GetPage(int pageIndex, int pageSize = 10, string orderBy = null)
        {
            return await _detachedContext.Set<TEntity>().LoadPageAsync(pageIndex, pageSize, q =>
            {
                q = Sorter<TEntity>.FromString(orderBy).Apply(q);
                return q;
            });
        }

        [HttpGet("{id}")]
        public virtual async Task<TEntity> FindById(string id)
        {
            return await _detachedContext.Set<TEntity>().LoadAsync(id);
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

    public class EntityController<TDbContext, TEntity, TQuery> : EntityController<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
        where TQuery : IQuery<TEntity>
    {
        IDetachedContext<TDbContext> _detachedContext;

        public EntityController(IDetachedContext<TDbContext> detachedContext) : base(detachedContext)
        {
            _detachedContext = detachedContext;
        }

        public virtual async Task<List<TEntity>> Search(TQuery query)
        {
            return await _detachedContext.Set<TEntity>().LoadAsync(q => query.Apply(q));
        }
    }
}
