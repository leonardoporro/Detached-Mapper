using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Binding
{
    public class BindCollectionTests
    {
        [Fact]
        public void Bind_Collection()
        {
            Mapper mapper = new Mapper();

            var expression = mapper.Bind<List<RootEntity>, List<RootDTO>>();

            var fn = expression.Compile();

            var dto = fn(new List<RootEntity> {
                new RootEntity
                {
                    Id = 1,
                    Name = "Entity"
                }
            });

            Assert.Equal(1, dto[0].Id);
            Assert.Equal("Entity", dto[0].Name);
        }

        [Fact]
        public void Bind_Collection_Property()
        {
            Mapper mapper = new Mapper();

            var expression = mapper.Bind<RootEntity, RootDTO>();

            var fn = expression.Compile();

            var dto = fn(new RootEntity
            {
                Id = 1,
                Name = "Entity",
                Reference = new List<ReferencedEntity> {
                    new ReferencedEntity
                    {
                        Id = 2,
                        Name = "SubEntity"
                    }
                }
            });

            Assert.Equal(1, dto.Id);
            Assert.Equal("Entity", dto.Name);
            Assert.NotNull(dto.Reference);
            Assert.Equal(2, dto.Reference[0].Id);
            Assert.Equal("SubEntity", dto.Reference[0].Name);
        }

        public class RootDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<ReferencedDTO> Reference { get; set; }
        }

        public class ReferencedDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class RootEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<ReferencedEntity> Reference { get; set; }
        }

        public class ReferencedEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}