using System;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Complex
{
    public class ComplexTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void map_same_type()
        {
            TargetTestType targetObj = new TargetTestType
            {
                Text = "sample text",
                Number = 5
            };

            TargetTestType mappedObj = mapper.Map<TargetTestType, TargetTestType>(targetObj);

            Assert.NotEqual(targetObj, mappedObj);
            Assert.Equal("sample text", mappedObj.Text);
            Assert.Equal(5, mappedObj.Number);
        }

        [Fact]
        public void map_different_types()
        {
            TargetTestType targetObj = new TargetTestType
            {
                Text = "sample text",
                Number = 5,
                DateTime = new DateTime(1984, 07, 09)
            };

            SourceTestType sourceObj = new SourceTestType
            {
                Text = "new text",
                Number = 9
            };

            TargetTestType mappedObj = mapper.Map(sourceObj, targetObj);

            Assert.Equal(targetObj, mappedObj);
            Assert.Equal("new text", mappedObj.Text);
            Assert.Equal(9, mappedObj.Number);
            Assert.Equal(new DateTime(1984, 07, 09), mappedObj.DateTime);
        }

        [Fact]
        public void map_anonymous_types()
        {
            TargetTestType targetObj = new TargetTestType
            {
                Text = "sample text",
                Number = 5,
                DateTime = new DateTime(1984, 07, 09)
            };

            var newObj = new
            {
                Text = "new text",
                Number = 9
            };

            TargetTestType mappedObj = mapper.Map(newObj, targetObj);

            Assert.Equal(targetObj, mappedObj);
            Assert.Equal("new text", mappedObj.Text);
            Assert.Equal(9, mappedObj.Number);
            Assert.Equal(new DateTime(1984, 07, 09), mappedObj.DateTime);
        }

        public class TargetTestType
        {
            public string Text { get; set; }

            public int Number { get; set; }

            public DateTime DateTime { get; set; }
        }

        public class SourceTestType
        {
            public string Text { get; set; }

            public int Number { get; set; }
        }
    }
}
