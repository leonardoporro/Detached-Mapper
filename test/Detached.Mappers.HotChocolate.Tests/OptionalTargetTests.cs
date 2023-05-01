using HotChocolate;
using Xunit;

namespace Detached.Mappers.HotChocolate.Tests
{
    public class OptionalTargetTests
    {
        [Fact]
        public void map_optional_target()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.WithHotChocolate();

            Mapper mapper = new Mapper(mapperOptions);

            DTO dto = new DTO()
            {
                Id = 1,
                Name = "dto",
                Nested =
                   new SubDTO
                   {
                       Id = 2,
                       Name = "subdto"
                   }
            };

            Entity entity = mapper.Map<DTO, Entity>(dto, null);

            Assert.Equal("dto", entity.Name); 
            Assert.Equal("subdto", entity.Nested.Value?.Name);
        }

        public class DTO
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public SubDTO Nested { get; set; }
        }

        public class SubDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class Entity
        {
            public int Id { get; set; }

            public Optional<string> Name { get; set; }

            public Optional<SubEntity> Nested { get; set; }
        }

        public class SubEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}