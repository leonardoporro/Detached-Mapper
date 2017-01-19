using Detached.EntityFramework.Plugins.Pagination;
using Detached.Mvc.Errors;
using Detached.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Server.Core
{
    [Route("api/[controller]")]
    public class DetachedController<TEntity, TQuery> : Controller
        where TEntity : class
        where TQuery : IDetachedQuery<TEntity>
    {
        #region Fields

        IDetachedService<TEntity, TQuery> _detachedService;

        #endregion

        #region Ctor.

        public DetachedController(IDetachedService<TEntity, TQuery> detachedService)
        {
            _detachedService = detachedService;
            bool x = new CultureInfo("es-AR") == new CultureInfo("es-AR");
        }

        #endregion

        [HttpGet("{id}")]
        public virtual async Task<TEntity> FindById(string id)
        {
            return await _detachedService.FindById(id);
        }

        [HttpGet]
        public virtual async Task<IList<TEntity>> Get(TQuery query = default(TQuery))
        {
            return await _detachedService.Get(query);
        }

        [HttpGet("pages/{pageIndex:int}")]
        public virtual async Task<IPage<TEntity>> GetPage(int pageIndex, int pageSize = 5, bool noCount = false, TQuery query = default(TQuery))
        {
            return await _detachedService.GetPage(pageIndex, pageSize, noCount, query);
        }

        [HttpPost]
        public virtual async Task<TEntity> Save([FromBody]TEntity entity)
        {
            ValidateModel();

            return await _detachedService.Save(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task Delete(string id)
        {
            await _detachedService.Delete(id);
        }

        protected virtual void ValidateModel()
        {
            if (!ModelState.IsValid)
            {
                ModelErrorResult modelErrorResult = new ModelErrorResult();

                throw new HandledException<ModelErrorResult>(modelErrorResult);
            }
        }
    }
}