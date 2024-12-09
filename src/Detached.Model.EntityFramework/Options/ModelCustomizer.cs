using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.Model.EntityFramework.Options
{
    public class ModelCustomizer<TDbContext> : IModelCustomizer
        where TDbContext : DbContext
    {
        readonly IEnumerable<IModelConfiguration<TDbContext>> _setups;

        public ModelCustomizer(IEnumerable<IModelConfiguration<TDbContext>> modelSetup)
        {
            _setups = modelSetup;
        }

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            if (_setups != null)
            {
                foreach (var modelSetup in _setups)
                {
                    modelSetup.ConfigureModel(modelBuilder, (TDbContext)context);
                }
            }
        }
    }
}