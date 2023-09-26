using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Mappers.EntityFramework.Features
{
    public class EntityMapperDbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly EntityMapperFactory _mapperFactory = new EntityMapperFactory();

        public EntityMapperDbContextOptionsExtension(Type dbContextType, Action<EntityMapperOptionsBuilder> configure)
        {
            Info = new EntityMapperDbContextOptionsExtensionInfo(this);
            if (configure != null)
            {
                _mapperFactory.RegisterConfigureAction(dbContextType, configure);
            }
        }

        public DbContextOptionsExtensionInfo Info { get; }


        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(_mapperFactory);
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}