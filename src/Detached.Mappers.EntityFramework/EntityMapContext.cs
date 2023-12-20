using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapContext : MapContext
    {
        public EntityMapContext(
            EntityMapperOptions mapperOptions,
            DbContext dbContext,  
            MapParameters parameters)
            : base(parameters)
        {
            Options = mapperOptions; 
            DbContext = dbContext; 
        }

        public EntityMapperOptions Options { get; } 

        public DbContext DbContext { get; } 
    }
}
