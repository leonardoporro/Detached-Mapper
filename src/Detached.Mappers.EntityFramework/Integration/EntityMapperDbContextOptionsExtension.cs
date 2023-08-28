using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Mappers.EntityFramework.Features
{
    public class EntityMapperDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public EntityMapperDbContextOptionsExtension(Type dbContextType, Action<EntityMapperOptionsBuilder> configure)
        {
            Info = new EntityMapperDbContextOptionsExtensionInfo(this);
            if (configure != null)
            {
                EntityMapperFactory.Instance.RegisterConfigureAction(dbContextType, configure);
            }
        }

        public DbContextOptionsExtensionInfo Info { get; }


        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(EntityMapperFactory.Instance);
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}