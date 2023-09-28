using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Loaders
{
    public class NoopQuery : ILoaderQuery
    {
        public object Load(DbContext dbContext, object entityOrDto)
        {
            return null;
        }

        public Task<object> LoadAsync(DbContext dbContext, object entityOrDto)
        {
            return Task.FromResult<object>(null);
        }
    }
}
