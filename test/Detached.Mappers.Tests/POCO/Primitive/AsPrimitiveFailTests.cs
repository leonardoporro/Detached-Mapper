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
            RootDTO dto = new RootDTO
            {
                Mapped = new InnerDTOClass { Name = "mapped class" },
                Copied = new InnerDTOClass { Name = "copied class" }
            };

            Assert.Throws<MapperException>(() => mapper.Map<RootDTO, RootEntity>(dto));
        }


        public class RootDTO
        {
            public InnerDTOClass Mapped { get; set; }

            public InnerDTOClass Copied { get; set; }
        }

        public class RootEntity
        {
            [Composition]
            public InnerEntityClass Mapped { get; set; }

            [Composition]
            [Primitive]
            public InnerEntityClass Copied { get; set; }
        }

        public class InnerDTOClass
        {
            public string Name { get; set; }
        }

        public class InnerEntityClass
        {
            public string Name { get; set; }
        }
    }
}
