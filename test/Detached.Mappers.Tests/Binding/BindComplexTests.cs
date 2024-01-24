using Xunit;

namespace Detached.Mappers.Tests.Binding
{
    public class BindComplexTests
    {
        [Fact]
        public void Bind_Complex()
        {
            Mapper mapper = new Mapper();

            var expression = mapper.Bind<RootEntity, RootDto>();

            var fn = expression.Compile();

            var dto = fn(new RootEntity
            {
                Id = 1,
                Name = "Entity"
            });

            Assert.Equal(1, dto.Id);
            Assert.Equal("Entity", dto.Name);
        }

        [Fact]
        public void Bind_Complex_Nested()
        {
            Mapper mapper = new Mapper();

            var expression = mapper.Bind<RootEntity, RootDto>();

            var fn = expression.Compile();

            var dto = fn(new RootEntity
            {
                Id = 1,
                Name = "Entity",
                Reference = new ReferencedEntity
                {
                    Id = 2,
                    Name = "SubEntity"
                }
            });

            Assert.Equal(1, dto.Id);
            Assert.Equal("Entity", dto.Name);
            Assert.NotNull(dto.Reference);
            Assert.Equal(2, dto.Reference.Id);
            Assert.Equal("SubEntity", dto.Reference.Name);
        }

        public class RootDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public ReferencedDto Reference { get; set; }
        }

        public class ReferencedDto
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class RootEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public ReferencedEntity Reference { get; set; }
        }

        public class ReferencedEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}