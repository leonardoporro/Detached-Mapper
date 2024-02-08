using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Loaders
{
    public interface ILoaderQuery
    {
        object Load(DbContext dbContext, object entityOrDto);

        IEnumerable<object> Load(DbContext dbContext,IEnumerable<object> entitiesOrDtos);

        Task<object> LoadAsync(DbContext dbContext, object entityOrDto);

        Task<IEnumerable<object>> LoadAsync(DbContext dbContext, IEnumerable<object> entityOrDto);
    }
}