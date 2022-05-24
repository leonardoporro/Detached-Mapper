using Detached.Mappers.Context;
using System;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.PrimitiveType
{
    public class NullableValueTest
    {
        [Fact]
        public void map_nullable_to_non_nullable()
        {
            Mapper mapper = new Mapper();

            Source source = new Source { Id = 1 };
            Target target = new Target();

            Target result = mapper.Map2(source, target, new MapperContext());
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void map_null_to_non_nullable()
        {
            Mapper mapper = new Mapper();

            Source source = new Source { Id = null };
            Target target = new Target();

            Target result = mapper.Map2(source, target, new MapperContext());
            Assert.Equal(0, result.Id);
        }

        public class Target
        {
            public int Id { get; set; }
        }

        public class Source
        {
            public int? Id { get; set; }
        }
    }
}