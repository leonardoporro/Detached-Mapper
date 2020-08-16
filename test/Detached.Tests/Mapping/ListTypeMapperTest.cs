using Detached.Mapping;
using System.Collections.Generic;
using Xunit;

namespace Detached.Tests.Mapping
{
    public class ListTypeMapperTest
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void MapCollection()
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