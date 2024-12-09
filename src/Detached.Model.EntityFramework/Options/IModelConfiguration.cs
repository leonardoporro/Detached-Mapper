using Microsoft.EntityFrameworkCore;

namespace Detached.Model.EntityFramework.Options
{
    public interface IModelConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        void ConfigureModel(ModelBuilder model, TDbContext dbContext);
    }
}
