using Detached.Annotations;
using Detached.Mapping;
using Detached.Mapping.Context;
using System.Collections.Generic;
using Xunit;

namespace Detached.Tests.Mapping.Entity
{
    public class BackReferenceTests
    {
        readonly Mapper mapper = new Mapper();

        [Fact]
        public void map_back_reference()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target_root",
                OwnedList = new List<TargetOwnedItem>
                {
                    new TargetOwnedItem { Id = 1, Name = "Item 1"},
                    new TargetOwnedItem { Id = 2, Name = "Item 2"},
                    new TargetOwnedItem { Id = 3, Name = "Item 3"},
                }
            };

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "target_root",
                OwnedList = new List<SourceOwnedItem>
                {
                    new SourceOwnedItem { Id = 1, Name = "Item 1"},
                    new SourceOwnedItem { Id = 2, Name = "Item 2"},
                    new SourceOwnedItem { Id = 3, Name = "Item 3"},
                }
            };

            MapperContext context = new MapperContext();

            var mapped = mapper.Map(source, target, context);

            Assert.Equal(mapped, mapped.OwnedList[0].Parent);
            Assert.Equal(mapped, mapped.OwnedList[1].Parent);
            Assert.Equal(mapped, mapped.OwnedList[2].Parent);
        }

        [Entity]
        public class TargetEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Composition]
            public List<TargetOwnedItem> OwnedList { get; set; }
        }

        [Entity]
        public class TargetOwnedItem
        {
            public TargetEntity Parent { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Composition]
            public List<SourceOwnedItem> OwnedList { get; set; }
        }

        public class SourceOwnedItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}