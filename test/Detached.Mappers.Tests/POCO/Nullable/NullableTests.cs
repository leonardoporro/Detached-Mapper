using Detached.Mappers.Context;
using System;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Nullable
{
    public class NullableTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void map_nullable_same_base()
        {
            Assert.Equal(0, mapper.Map2<int?, int>(null));
            
            Assert.Equal(1, mapper.Map2<int?, int>(new int?(1)));

            Assert.Equal(1, mapper.Map2<int, int?>(1));

            Assert.Equal(1, mapper.Map2<int?, int?>(1));
        }

        [Fact]
        public void map_nullable_different_base()
        {
            Assert.Null(mapper.Map2<string, int?>(null));

            Assert.Equal(1, mapper.Map2<string, int?>("1"));

            Assert.Null(mapper.Map2<int?, string>(null));

            Assert.Equal("1", mapper.Map2<int?, string>(1));
        }
    }
}