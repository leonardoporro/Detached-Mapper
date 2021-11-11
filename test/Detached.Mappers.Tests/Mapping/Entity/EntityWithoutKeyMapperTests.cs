using Detached.Annotations;
using Detached.Mappers.Context;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.Entity
{
    public class EntityWithoutKeyMapperTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public Task map_dto_without_key()
        {
            Entity entity = new Entity
            {
                Id = 1,
                Name = "the entity"
            };

            NoKeyDTO dto = new NoKeyDTO
            {
                Name = "the dto"
            };
            
            MapperContext context = new MapperContext();
            mapper.Map(dto, entity, context);

            Assert.Equal("the dto", entity.Name);

            return Task.CompletedTask;
        }

        [Entity]
        public class Entity
        {
            [Key]
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class NoKeyDTO
        {
            public string Name { get; set; }
        }
    }
}
