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

            Dto dto = new Dto()
            {
                Id = 1,
                Name = "dto",
                Nested =
                   new SubDto
                   {
                       Id = 2,
                       Name = "subdto"
                   }
            };

            Entity entity = mapper.Map<Dto, Entity>(dto, null);

            Assert.Equal("dto", entity.Name); 
            Assert.Equal("subdto", entity.Nested.Value?.Name);
        }

        public class Dto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public SubDto Nested { get; set; }
        }

        public class SubDto
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