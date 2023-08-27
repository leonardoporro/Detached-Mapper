using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public interface IEntityMapperCustomizer
    {
        void Customize(DbContext dbContext, object profileKey, MapperOptions mapperOptions);
    }
}
