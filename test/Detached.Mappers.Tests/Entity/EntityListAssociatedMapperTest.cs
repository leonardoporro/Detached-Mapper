using Detached.Annotations;
using Detached.Mappers.Context;
using Detached.Mappers.TypeMappers.Entity;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Entity
{
    public class AssociatedCollectionMapperTests
    {
        readonly Mapper mapper = new Mapper();

        [Fact]
        public void map_associated_collection()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target_root",
                AssociatedList = new List<TargetAssociatedItem>
                {
                    new TargetAssociatedItem { Id = 1, Name = "Item 1"},
                    new TargetAssociatedItem { Id = 2, Name = "Item 2"},
                    new TargetAssociatedItem { Id = 3, Name = "Item 3"},
                }
            };

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "target_root",
                AssociatedList = new List<SourceAssociatedItem>
                {
                    new SourceAssociatedItem { Id = 2, Name = "Item 2"},
                    new SourceAssociatedItem { Id = 4, Name = "Item 4"},
                }
            };

            MapContext context = new MapContext();

            var mapped = mapper.Map(source, target, context);
            Assert.NotNull(mapped.AssociatedList);
            Assert.Equal(2, mapped.AssociatedList.Count);

            Assert.True(context.TryGetEntry<TargetAssociatedItem>(new EntityKey<int>(4), out MapperContextEntry entry4));
            Assert.Equal(MapperActionType.Attach, entry4.ActionType);
        }

        [Entity]
        public class TargetEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<TargetAssociatedItem> AssociatedList { get; set; }
        }

        [Entity]
        public class TargetAssociatedItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<SourceAssociatedItem> AssociatedList { get; set; }
        }

        public class SourceAssociatedItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}