using Detached.Mapping;
using Detached.Model;
using Microsoft.Extensions.Options;
using Xunit;

namespace Detached.Tests.Model
{
    public class ConstructorTests
    {
        [Fact]
        public void customize_constructor()
        {
            ModelOptions modelOptions = new ModelOptions();
            modelOptions.Configure<TargetEntity>().Constructor(c => new TargetEntity(1));

            Mapper mapper = new Mapper(
                Options.Create(modelOptions), 
                new TypeMapFactory());

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
