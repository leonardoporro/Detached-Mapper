using Detached.Annotations;
using Detached.Mappers.Context;
using Detached.Mappers.TypeMaps;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.EntityType
{
    public class EntityListCompositionMapperTests
    {
        readonly Mapper mapper = new Mapper();

        [Fact]
        public void map_composition_collection()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target_root",
                CompositionList = new List<TargetCompositionItem>
                {
                    new TargetCompositionItem { Id = 1, Name = "Item 1"},
                    new TargetCompositionItem { Id = 2, Name = "Item 2"},
                    new TargetCompositionItem { Id = 3, Name = "Item 3"},
                }
            };

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "target_root",
                CompositionList = new List<SourceCompositionItem>
                {
                    new SourceCompositionItem { Id = 2, Name = "Item 2"},
                    new SourceCompositionItem { Id = 4, Name = "Item 4"},
                }
            };

            MapperContext context = new MapperContext();

            TargetEntity mapped = mapper.Map(source, target, context);

            Assert.NotNull(mapped.CompositionList);
            Assert.Equal(2, mapped.CompositionList.Count);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(1), out MapperContextEntry entry1));
            Assert.Equal(MapperActionType.Delete, entry1.ActionType);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(2), out MapperContextEntry entry2));
            Assert.Equal(MapperActionType.Update, entry2.ActionType);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(3), out MapperContextEntry entry3));
            Assert.Equal(MapperActionType.Delete, entry3.ActionType);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(4), out MapperContextEntry entry4));
            Assert.Equal(MapperActionType.Create, entry4.ActionType);
        }

        [Fact]
        public void map_composition_collection_null_keys()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target_root",
                CompositionList = new List<TargetCompositionItem>
                {
                    new TargetCompositionItem { Id = 1, Name = "Item 1"},
                    new TargetCompositionItem { Id = 2, Name = "Item 2"},
                    new TargetCompositionItem { Id = 3, Name = "Item 3"},
                }
            };

            SourceEntityStringKey source = new SourceEntityStringKey
            {
                Id = "1",
                Name = "target_root",
                CompositionList = new List<SourceCompositionItemStringKey>
                {
                    new SourceCompositionItemStringKey { Id = "2", Name = "Item 2"},
                    new SourceCompositionItemStringKey { Name = "Item 4"}, // new has null key.
                }
            };

            MapperContext context = new MapperContext();

            TargetEntity mapped = mapper.Map(source, target, context);

            Assert.NotNull(mapped.CompositionList);
            Assert.Equal(2, mapped.CompositionList.Count);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(1), out MapperContextEntry entry1));
            Assert.Equal(MapperActionType.Delete, entry1.ActionType);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(2), out MapperContextEntry entry2));
            Assert.Equal(MapperActionType.Update, entry2.ActionType);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(3), out MapperContextEntry entry3));
            Assert.Equal(MapperActionType.Delete, entry3.ActionType);

            Assert.True(context.TryGetEntry<TargetCompositionItem>(new EntityKey<int>(0), out MapperContextEntry entry4));
            Assert.Equal(MapperActionType.Create, entry4.ActionType);
        }

        [Entity]
        public class TargetEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Composition]
            public List<TargetCompositionItem> CompositionList { get; set; }
        }

        [Entity]
        public class TargetCompositionItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<SourceCompositionItem> CompositionList { get; set; }
        }

        public class SourceCompositionItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntityStringKey
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public List<SourceCompositionItemStringKey> CompositionList { get; set; }
        }

        public class SourceCompositionItemStringKey
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}