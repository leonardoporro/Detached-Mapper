using Xunit;

namespace Detached.Mappers.Tests.Class.Primitive
{
    public class MapPrimitive
    {
        [Fact]
        public void map_string_to_int()
        {
            Mapper mapper = new Mapper();

            int result = mapper.Map<string, int>("1");

            Assert.Equal(1, result);
        }

        [Fact]
        public void map_string_to_string()
        {
            Mapper mapper = new Mapper();

            string result = mapper.Map<string, string>("1");

            Assert.Equal("1", result);
        }

        [Fact]
        public void map_string_to_bool()
        {
            Mapper mapper = new Mapper();

            bool result = mapper.Map<string, bool>("true");

            Assert.True(result);
        }
    }
}
