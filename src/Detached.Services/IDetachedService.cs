using Detached.EntityFramework.Plugins.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Services
{
    public interface IDetachedService<TEntity, TQuery>
        where TEntity : class
        where TQuery : IDetachedQuery<TEntity>
    {
        Task<TEntity> FindById(object key);

        Task<IList<TEntity>> Get(TQuery query = default(TQuery));

        Task<IPage<TEntity>> GetPage(int pageIndex, int pageSize = 5, bool noCount = false, TQuery query = default(TQuery));

        Task<TEntity> Save(TEntity entity);

        Task Delete(object key);
    }
}
