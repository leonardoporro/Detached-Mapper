using Microsoft.EntityFrameworkCore;

namespace Detached.Models.EntityFramework.Options
{
    public class DelegateModelConfiguration<TDbContext>(Action<ModelBuilder, TDbContext> configure) : IModelConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        public void ConfigureModel(ModelBuilder model, TDbContext dbContext)
        {
            configure(model, dbContext);
        }
    }
}