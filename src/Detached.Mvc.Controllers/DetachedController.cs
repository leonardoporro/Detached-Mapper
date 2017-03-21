using Detached.EntityFramework.Plugins.Pagination;
using Detached.Mvc.Validation;
using Detached.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Mvc.Controllers
{
    [Route("api/[controller]")]
    public abstract class DetachedController<TEntity, TQuery> : Controller
        where TEntity : class
        where TQuery : IDetachedQuery<TEntity>
    {
        #region Fields

        IDetachedService<TEntity, TQuery> _detachedService;
        IStringLocalizer _stringLocalizer;
        ILogger _logger;

        #endregion

        #region Ctor.

        public DetachedController(IDetachedService<TEntity, TQuery> detachedService)
        {
            _detachedService = detachedService;
        }

        #endregion

        [HttpGet("{id}")]
        public virtual async Task<TEntity> FindById(string id)
        {
            await Task.Delay(2000);

            return await _detachedService.FindById(id);
        }

        [HttpGet]
        public virtual async Task<IList<TEntity>> Get(TQuery query = default(TQuery))
        {
            await Task.Delay(2000);

            return await _detachedService.Get(query);
        }

        [HttpGet("pages/{pageIndex:int}")]
        public virtual async Task<IPage<TEntity>> GetPage(int pageIndex, int pageSize = 5, bool noCount = false, TQuery query = default(TQuery))
        {
            await Task.Delay(2000);

            return await _detachedService.GetPage(pageIndex, pageSize, noCount, query);
        }

        [HttpPost] 
        public virtual async Task<TEntity> Save([FromBody]TEntity entity)
        {
            return await _detachedService.Save(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task Delete(string id)
        {
            await _detachedService.Delete(id);
        }
    }
}
