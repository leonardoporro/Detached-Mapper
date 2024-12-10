using Microsoft.EntityFrameworkCore;

namespace Detached.Models.EntityFramework.Options
{
    public interface IModelConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        void ConfigureModel(ModelBuilder model, TDbContext dbContext);
    }
}
