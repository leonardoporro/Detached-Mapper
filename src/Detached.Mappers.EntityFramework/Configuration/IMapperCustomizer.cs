using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public interface IMapperCustomizer
    {
        void Customize(DbContext dbContext, MapperOptions mapperOptions);
    }
}
