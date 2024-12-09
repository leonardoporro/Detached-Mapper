using Microsoft.EntityFrameworkCore;

namespace Detached.Model.EntityFramework.Options
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