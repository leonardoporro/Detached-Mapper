using Detached.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.ComplexType
{
    public class NotMappedTests
    {
        readonly Mapper _mapper = new Mapper();

        [Fact]
        public void ignore_not_mapped_property()
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

            var mapped = _mapper.Map2(source, target);

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
