using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Complex.Collection
{
    public class CollectionTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void map_primitive_collection()
        {
            List<int> ints = new List<int> { 1, 2, 3, 4, 5 };

            List<string> result = mapper.Map<List<int>, List<string>>(ints);

            Assert.Equal("1", result[0]);
            Assert.Equal("2", result[1]);
            Assert.Equal("3", result[2]);
            Assert.Equal("4", result[3]);
            Assert.Equal("5", result[4]);
        }

        [Fact]
        public void map_complex_collection()
        {
            SourceType sourceType = new SourceType
            {
                ComplexCollection = new List<SourceItemType>
                {
                    new SourceItemType { Name = "Item 1" },
                    new SourceItemType { Name = "Item 2" },
                    new SourceItemType { Name = "Item 3" }
                }
            };

            TargetType targetType = mapper.Map<SourceType, TargetType>(sourceType);

            Assert.NotNull(targetType.ComplexCollection);
            Assert.Equal("Item 1", targetType.ComplexCollection[0].Name);
            Assert.Equal("Item 2", targetType.ComplexCollection[1].Name);
            Assert.Equal("Item 3", targetType.ComplexCollection[2].Name);
        }

        [Fact]
        public void map_list_to_collection()
        {
            List<int> ints = new List<int> { 1, 2, 3, 4, 5 };

            Collection<string> result = mapper.Map<List<int>, Collection<string>>(ints);

            Assert.Equal("1", result[0]);
            Assert.Equal("2", result[1]);
            Assert.Equal("3", result[2]);
            Assert.Equal("4", result[3]);
            Assert.Equal("5", result[4]);
        }

        public class TargetType
        {
            public List<TargetItemType> ComplexCollection { get; set; }
        }

        public class TargetItemType
        {
            public string Name { get; set; }
        }

        public class SourceType
        {
            public List<SourceItemType> ComplexCollection { get; set; }
        }

        public class SourceItemType
        {
            public string Name { get; set; }
        }
    }
}