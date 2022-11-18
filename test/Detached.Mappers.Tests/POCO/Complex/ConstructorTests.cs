using Xunit;

namespace Detached.Mappers.Tests.POCO.Complex
{
    public class ConstructorTests
    {
        [Fact]
        public void customize_constructor()
        {
            MapperOptions modelOptions = new MapperOptions();
            modelOptions.Type<TargetEntity>().Constructor(c => new TargetEntity(1));

            Mapper mapper = new Mapper(modelOptions);

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
