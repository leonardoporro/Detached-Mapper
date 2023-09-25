using Xunit;

namespace Detached.Mappers.Tests.Projections
{
    public class TypeBinderTests
    {
        [Fact]
        public void TypeBinder_ShouldBuildProjection()
        {
            Mapper mapper = new Mapper();

            var expression = mapper.Bind<Entity, DTO>();
        }

        public class DTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class Entity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}