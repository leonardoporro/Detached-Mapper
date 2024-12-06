using Xunit;

namespace Detached.Mappers.Tests.Class.Primitive
{
    public class MapNullablePrimitives
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void map_nullable_same_base()
        {
            Assert.Equal(0, mapper.Map<int?, int>(null));

            Assert.Equal(1, mapper.Map<int?, int>(new int?(1)));

            Assert.Equal(1, mapper.Map<int, int?>(1));

            Assert.Equal(1, mapper.Map<int?, int?>(1));
        }

        [Fact]
        public void map_nullable_different_base()
        {
            Assert.Null(mapper.Map<string, int?>(null));

            Assert.Equal(1, mapper.Map<string, int?>("1"));

            Assert.Null(mapper.Map<int?, string>(null));

            Assert.Equal("1", mapper.Map<int?, string>(1));
        }
    }
}