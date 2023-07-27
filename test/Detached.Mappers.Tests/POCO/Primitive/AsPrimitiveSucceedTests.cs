using Detached.Annotations;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Primitive
{
    public class AsPrimitiveSucceedTests
    {
        Mapper mapper = new Mapper();


        [Fact]
        public void map_as_primitive_succeeds()
        {
            RootDTO dto = new RootDTO
            {
                Mapped = new InnerClass { Name = "mapped class" },
                Copied = new InnerClass { Name = "copied class" }
            };

            var result = mapper.Map<RootDTO, RootEntity>(dto);

            Assert.Equal("copied class", result.Copied.Name);
            Assert.Equal("mapped class", result.Mapped.Name);

            Assert.NotEqual(result.Mapped, dto.Mapped);
            Assert.Equal(result.Copied, dto.Copied);
        }

        public class RootDTO
        {
            public InnerClass Mapped { get; set; }

            public InnerClass Copied { get; set; }
        }

        public class RootEntity
        {
            [Composition]
            public InnerClass Mapped { get; set; }

            [Composition]
            [Primitive]
            public InnerClass Copied { get; set; }
        }

        public class InnerClass
        {
            public string Name { get; set; }
        }
    }
}
