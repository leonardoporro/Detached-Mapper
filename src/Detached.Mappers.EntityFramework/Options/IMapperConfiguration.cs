using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Options
{
    public interface IMapperConfiguration
    {
        void ConfigureMapper(EntityMapperOptionsBuilder mapper);
    }

    public interface IMapperConfiguration<TDbContext> : IMapperConfiguration
        where TDbContext : DbContext
    {
    }
}