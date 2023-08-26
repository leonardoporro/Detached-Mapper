using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EFMapperDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public EFMapperDbContextOptionsExtension(Action<EFMapperConfigurationBuilder> configure)
        {
            Info = new EFMapperDbContextOptionsExtensionInfo(this);
            Configure = configure;
        }

        public DbContextOptionsExtensionInfo Info { get; }

        public Action<EFMapperConfigurationBuilder> Configure { get; } 

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(new EFMapperProfiles(Configure));
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}