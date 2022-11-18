using Detached.Mappers.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class DetachedDbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly DetachedConfigurationBuilder _configBuilder;

        public DetachedDbContextOptionsExtension(DetachedConfigurationBuilder configBuilder)
        {
            Info = new DetachedDbContextOptionsExtensionInfo(this);
            _configBuilder = configBuilder;
        }

        internal DbContextMapperCollection Profiles { get; set; }

        public DbContextOptionsExtensionInfo Info { get; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(new DbContextMapperCollection(_configBuilder));
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}