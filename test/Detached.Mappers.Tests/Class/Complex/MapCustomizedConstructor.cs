using Detached.Mappers.Options;
using Xunit;

namespace Detached.Mappers.Tests.Class.Complex
{
    public class MapCustomizedConstructor
    {
        [Fact]
        public void map_customized_constructor()
        {
            var options = new MapperOptionsBuilder()
                .Type<TargetEntity>()
                    .Constructor(c => new TargetEntity(1))
                .Options;

            Mapper mapper = new Mapper(options);

            var result = mapper.Map<SourceEntity, TargetEntity>(new SourceEntity { Value = 2 });

            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.Value);
        }

        public class SourceEntity
        {
            public int Value { get; set; }
        }

        public class TargetEntity
        {
            public TargetEntity(int id)
            {
                Id = id;
            }

            public int Id { get; }

            public int Value { get; set; }
        }
    }
}
