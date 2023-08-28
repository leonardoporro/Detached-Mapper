using Detached.Annotations;
using Detached.Mappers.Tests.Mocks;
using Detached.Mappers.TypeMappers.Entity;
using Detached.Mappers.TypeMappers.Entity.Collection;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Entity
{
    public class EntityCollectionNullMapperTest
    {
        /// <summary>
        /// Test default behaviour of mapper (clear target collection if source is null)
        /// </summary>
        [Fact]
        public void map_null_collection_default()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target_root",
                ItemList = new List<TargetItem>
                {
                    new TargetItem { Id = 1, Name = "Item 1"},
                    new TargetItem { Id = 2, Name = "Item 2"},
                    new TargetItem { Id = 3, Name = "Item 3"},
                }
            };

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "target_root",
                ItemList = null
            };

            Mapper defaultMapper = new Mapper();
            var mapped = defaultMapper.Map(source, target, new MapContextMock());
            Assert.Empty(mapped.ItemList);
        }

        /// <summary>
        /// Test default behaviour of mapper (clear target collection if source is null)
        /// </summary>
        [Fact]
        public void map_null_collection_ignore()
        {
            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "target_root",
                ItemList = new List<TargetItem>
                {
                    new TargetItem { Id = 1, Name = "Item 1"},
                    new TargetItem { Id = 2, Name = "Item 2"},
                    new TargetItem { Id = 3, Name = "Item 3"},
                }
            };

            SourceEntity source = new SourceEntity
            {
                Id = 1,
                Name = "target_root",
                ItemList = null
            };

            Mapper ignoreNullMapper = new Mapper(new MapperOptions() { EntityCollectionNullBehavior = EntityCollectionNullBehavior.Ignore });
            TargetEntity mapped = ignoreNullMapper.Map(source, target, new MapContextMock());
            Assert.NotEmpty(mapped.ItemList);
            Assert.Equal(3, mapped.ItemList.Count);
        }

        [Entity]
        public class TargetEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<TargetItem> ItemList { get; set; }
        }

        [Entity]
        public class TargetItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<SourceItem> ItemList { get; set; }
        }

        public class SourceItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}