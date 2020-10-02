using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers
{
    public interface IMapperCustomizer
    {
        void Customize(DbContext dbContext, MapperOptions mapperOptions);
    }
}
