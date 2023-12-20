using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Loaders
{
    public interface ILoaderQuery
    {
        object Load(DbContext dbContext, object entityOrDto);

        Task<object> LoadAsync(DbContext dbContext, object entityOrDto);
    }
}