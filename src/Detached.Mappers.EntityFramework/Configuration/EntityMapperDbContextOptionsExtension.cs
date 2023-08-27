using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EntityMapperDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public EntityMapperDbContextOptionsExtension(EntityMapperServices services)
        {
            Info = new EntityMapperDbContextOptionsExtensionInfo(this);
            EntityMapperServices = services;
        }

        public DbContextOptionsExtensionInfo Info { get; }

        public EntityMapperServices EntityMapperServices { get; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(EntityMapperServices);
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}