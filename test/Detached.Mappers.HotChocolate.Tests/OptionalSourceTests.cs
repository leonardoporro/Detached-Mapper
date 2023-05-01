using Detached.Mappers;
using Detached.Mappers.HotChocolate;
using HotChocolate;
using Xunit;

namespace Detached.Mappers.HotChocolate.Tests
{
    public class OptionalSourceTests
    {
        [Fact]
        public void map_optional_source()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.WithHotChocolate();

            Mapper mapper = new Mapper(mapperOptions);

            DTO dto = new DTO()
            {
                Id = 1,
                Name = "dto",
                Nested = new Optional<SubDTO>(
                   new SubDTO
                   {
                       Id = 2,
                       Name = "subdto"
                   }
                )
            };

            Entity entity = mapper.Map<DTO, Entity>(dto, null);

            Assert.Equal("dto", entity.Name);
            Assert.NotNull(entity.Nested);
            Assert.Equal("subdto", entity.Nested.Name);
        }

        public class DTO
        {
            public int Id { get; set; }

            public Optional<string> Name { get; set; }

            public Optional<SubDTO> Nested { get; set; }
        }

        public class SubDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class Entity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public SubEntity Nested { get; set; }
        }

        public class SubEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}