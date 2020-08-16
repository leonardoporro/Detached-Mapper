using Detached.Annotations;
using Detached.Mapping;
using Detached.Mapping.Context;
using System.Collections.Generic;
using Xunit;

namespace Detached.Tests.Mapping.Entity
{
    public class EntityListOwnedMapperTests
    {
        readonly Mapper mapper = new Mapper();

        [Fact]
        public void map_owned_collection()
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
                    new SourceOwnedItem { Id = 2, Name = "Item 2"},
                    new SourceOwnedItem { Id = 4, Name = "Item 4"},
                }
            };

            MapperContext context = new MapperContext();

            TargetEntity mapped = mapper.Map(source, target, context);
 
            Assert.NotNull(mapped.OwnedList);
            Assert.Equal(2, mapped.OwnedList.Count);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(1), out MapperContextEntry entry1));
            Assert.Equal(MapperActionType.Delete, entry1.ActionType);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(2), out MapperContextEntry entry2));
            Assert.Equal(MapperActionType.Update, entry2.ActionType);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(3), out MapperContextEntry entry3));
            Assert.Equal(MapperActionType.Delete, entry3.ActionType);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(4), out MapperContextEntry entry4));
            Assert.Equal(MapperActionType.Create, entry4.ActionType);
        }

        [Fact]
        public void map_owned_collection_null_keys()
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

            SourceEntityStringKey source = new SourceEntityStringKey
            {
                Id = "1",
                Name = "target_root",
                OwnedList = new List<SourceOwnedItemStringKey>
                {
                    new SourceOwnedItemStringKey { Id = "2", Name = "Item 2"},
                    new SourceOwnedItemStringKey { Name = "Item 4"}, // new has null key.
                }
            };

            MapperContext context = new MapperContext();

            TargetEntity mapped = mapper.Map(source, target, context);

            Assert.NotNull(mapped.OwnedList);
            Assert.Equal(2, mapped.OwnedList.Count);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(1), out MapperContextEntry entry1));
            Assert.Equal(MapperActionType.Delete, entry1.ActionType);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(2), out MapperContextEntry entry2));
            Assert.Equal(MapperActionType.Update, entry2.ActionType);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(3), out MapperContextEntry entry3));
            Assert.Equal(MapperActionType.Delete, entry3.ActionType);

            Assert.True(context.TryGetEntry<TargetOwnedItem>(new EntityKey<int>(0), out MapperContextEntry entry4));
            Assert.Equal(MapperActionType.Create, entry4.ActionType);
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
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<SourceOwnedItem> OwnedList { get; set; }
        }

        public class SourceOwnedItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntityStringKey
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public List<SourceOwnedItemStringKey> OwnedList { get; set; }
        }

        public class SourceOwnedItemStringKey
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}