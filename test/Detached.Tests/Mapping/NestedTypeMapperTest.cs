using Detached.Mapping;
using Xunit;

namespace Detached.Tests.Mapping
{
    public class NestedTypeMapperTest
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void MapSameType()
        {
            NestedTargetType oldObj = new NestedTargetType
            {
                Text = "sample text",
                Number = 5,
                Nested = new NestedChildTargetType
                {
                    Number = 9,
                    Text = "sample nested"
                }
            };

            NestedTargetType mappedObj = mapper.Map<NestedTargetType, NestedTargetType>(oldObj);

            Assert.NotEqual(oldObj, mappedObj);
            Assert.Equal("sample text", mappedObj.Text);
            Assert.Equal(5, mappedObj.Number);
            Assert.NotNull(mappedObj.Nested);
            Assert.Equal("sample nested", mappedObj.Nested.Text);
            Assert.Equal(9, mappedObj.Nested.Number);
        }

        [Fact]
        public void MergeOtherType()
        {
            NestedTargetType targetObj = new NestedTargetType
            {
                Text = "sample text",
                Number = 5,
                Nested = new NestedChildTargetType
                {
                    Number = 9,
                    Text = "sample nested"
                }
            };

            NestedSourceType sourceObj = new NestedSourceType
            {
                Text = "new text",
                Number = 9,
                Nested = new NestedChildSourceType
                {
                    Number = 1,
                    Text = "other sample nested"
                }
            };

            NestedTargetType mappedObj = mapper.Map(sourceObj, targetObj);

            Assert.Equal(targetObj, mappedObj);
            Assert.Equal("new text", mappedObj.Text);
            Assert.Equal(9, mappedObj.Number);
            Assert.NotNull(mappedObj.Nested);
            Assert.Equal("other sample nested", mappedObj.Nested.Text);
            Assert.Equal(1, mappedObj.Nested.Number);
        }

        [Fact]
        public void MergeOtherNullValue()
        {
            NestedTargetType targetObj = new NestedTargetType
            {
                Text = "sample text",
                Number = 5,
                Nested = new NestedChildTargetType
                {
                    Number = 9,
                    Text = "sample nested"
                }
            };

            NestedSourceType sourceObj = new NestedSourceType
            {
                Text = "new text",
                Number = 9,
                Nested = null
            };

            NestedTargetType mappedObj = mapper.Map(sourceObj, targetObj);

            Assert.Equal(targetObj, mappedObj);
            Assert.Equal("new text", mappedObj.Text);
            Assert.Equal(9, mappedObj.Number);
            Assert.Null(mappedObj.Nested);
        }

        [Fact]
        public void MergeAnonymousType()
        {
            NestedTargetType targetObj = new NestedTargetType
            {
                Text = "sample text",
                Number = 5,
                Nested = new NestedChildTargetType
                {
                    Number = 9,
                    Text = "sample nested"
                }
            };

            var newObj = new
            {
                Text = "new text",
                Number = 9,
                Nested = new
                {
                    Number = 1,
                    Text = "other sample nested"
                }
            };

            NestedTargetType mappedObj = mapper.Map(newObj, targetObj);

            Assert.Equal(targetObj, mappedObj);
            Assert.Equal("new text", mappedObj.Text);
            Assert.Equal(9, mappedObj.Number);
            Assert.NotNull(mappedObj.Nested);
            Assert.Equal("other sample nested", mappedObj.Nested.Text);
            Assert.Equal(1, mappedObj.Nested.Number);
        }

        public class NestedTargetType
        {
            public string Text { get; set; }

            public int Number { get; set; }

            public NestedChildTargetType Nested { get; set; }
        }

        public class NestedChildTargetType
        {
            public string Text { get; set; }

            public int Number { get; set; }
        }

        public class NestedSourceType
        {
            public string Text { get; set; }

            public int Number { get; set; }

            public NestedChildSourceType Nested { get; set; }
        }

        public class NestedChildSourceType
        {
            public string Text { get; set; }

            public int Number { get; set; }
        }
    }
}
