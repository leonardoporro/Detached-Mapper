using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Options
{
    public class DelegateMapperConfiguration<TDbContext>(Action<EntityMapperOptionsBuilder> config) : IMapperConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        public void ConfigureMapper(EntityMapperOptionsBuilder mapper)
        {
            config(mapper);
        }
    }
}
