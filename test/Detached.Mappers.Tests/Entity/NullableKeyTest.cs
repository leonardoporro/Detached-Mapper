using System;
using Xunit;

namespace Detached.Mappers.Tests.Entity
{
    public class NullableKeyTest
    {
        readonly Mapper _mapper = new Mapper();

        [Fact]
        public void MapNullableKey()
        {
            Entity target = new Entity();
            EntityDto source = new EntityDto
            {
                Id = Guid.NewGuid(),
                Name = "the dto"
            };

            var mapped = _mapper.Map(source, target);

            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.Name, target.Name);
        }

        public class EntityDto
        {
            public Guid? Id { get; set; }

            public string Name { get; set; }
        }

        public class Entity
        {
            public Guid? Id { get; set; }

            public string Name { get; set; }
        }
    }
}
