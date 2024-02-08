using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Loaders
{
    public class NoopQuery : ILoaderQuery
    {
        public object Load(DbContext dbContext, object entityOrDto)
        {
            return null;
        }

        public IEnumerable<object> Load(DbContext dbContext, IEnumerable<object> entitiesOrDtos)
        {
            return Array.Empty<object>();
        }

        public Task<object> LoadAsync(DbContext dbContext, object entityOrDto)
        {
            return Task.FromResult<object>(null);
        }

        public Task<IEnumerable<object>> LoadAsync(DbContext dbContext, IEnumerable<object> entityOrDto)
        {
            return Task.FromResult<IEnumerable<object>>(Array.Empty<object>());
        }
    }
}
