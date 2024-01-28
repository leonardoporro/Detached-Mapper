using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapContext : MapContext
    {
        public EntityMapContext(
            MapperOptions mapperOptions,
            DbContext dbContext,  
            MapParameters parameters)
            : base(parameters)
        {
            Options = mapperOptions; 
            DbContext = dbContext; 
        }

        public MapperOptions Options { get; } 

        public DbContext DbContext { get; } 
    }
}
