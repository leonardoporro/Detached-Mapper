using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Models.EntityFramework.Options
{
    public class ModelDbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly IModelCustomizer _modelCustomizer;

        public ModelDbContextOptionsExtension(Type dbContextType, IModelCustomizer modelCustomizer)
        {
            Info = new ModelDbContextOptionsExtensionInfo(this);

            _modelCustomizer = modelCustomizer;
        }

        public DbContextOptionsExtensionInfo Info { get; }


        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(_modelCustomizer);
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}