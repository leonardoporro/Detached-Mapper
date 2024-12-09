using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Mappers.EntityFramework.Options
{
    public class EntityMapperDbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly static EntityMapperFactory _mapperFactory = new EntityMapperFactory();

        public EntityMapperDbContextOptionsExtension(Type dbContextType, EntityMapperOptions options)
        {
            Info = new EntityMapperDbContextOptionsExtensionInfo(this);

            _mapperFactory.Configure(dbContextType, options);
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