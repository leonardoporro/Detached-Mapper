using Detached.Annotations;
using Detached.Mappers.Exceptions;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Primitive
{
    public class AsPrimitiveFailTests
    {
        Mapper mapper = new Mapper();


        [Fact]
        public void map_as_primitive_fails_different_types()
        {
            RootDto dto = new RootDto
            {
                Mapped = new InnerDtoClass { Name = "mapped class" },
                Copied = new InnerDtoClass { Name = "copied class" }
            };

            Assert.Throws<MapperException>(() => mapper.Map<RootDto, RootEntity>(dto));
        }


        public class RootDto
        {
            public InnerDtoClass Mapped { get; set; }

            public InnerDtoClass Copied { get; set; }
        }

        public class RootEntity
        {
            [Composition]
            public InnerEntityClass Mapped { get; set; }

            [Composition]
            [Primitive]
            public InnerEntityClass Copied { get; set; }
        }

        public class InnerDtoClass
        {
            public string Name { get; set; }
        }

        public class InnerEntityClass
        {
            public string Name { get; set; }
        }
    }
}
