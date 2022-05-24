using Xunit;

namespace Detached.Mappers.Tests.Mapping.PrimitiveType
{
    public class PrimitiveTypeTest
    {
        [Fact]
        public void map_string_to_int()
        {
            Mapper mapper = new Mapper();

            int result = mapper.Map2<string, int>("1");

            Assert.Equal(1, result);
        }

        [Fact]
        public void map_string_to_string()
        {
            Mapper mapper = new Mapper();

            string result = mapper.Map2<string, string>("1");

            Assert.Equal("1", result);
        }

        [Fact]
        public void map_string_to_bool()
        {
            Mapper mapper = new Mapper();

            bool result = mapper.Map2<string, bool>("true");

            Assert.True(result);
        }
    }
}
