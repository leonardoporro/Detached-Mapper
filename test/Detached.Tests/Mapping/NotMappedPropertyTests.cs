using Detached.Annotations;
using Detached.Mapping;
using Xunit;

namespace Detached.Tests.Mapping
{
    public class NotMappedPropertyTests
    {
        readonly Mapper _mapper = new Mapper();
    
        [Fact]
        public void IgnoreNotMap()
        {
            Entity source = new Entity
            {
                Text1 = "target_text1",
                Text2 = "target_text2"
            };

            Entity target = new Entity
            {
                Text1 = "source_text1",
                Text2 = "source_text2"
            };

            var mapped =_mapper.Map(source, target);

            Assert.Equal("target_text1", mapped.Text1);
            Assert.Equal("source_text2", mapped.Text2);
        }

        public class Entity
        {
            public string Text1 { get; set; }

            [NotMapped]
            public string Text2 { get; set; }
        }
    }
}
