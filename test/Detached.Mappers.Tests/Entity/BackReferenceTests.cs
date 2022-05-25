using Detached.Annotations;
using Detached.Mappers.Context;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.EntityType
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
                    new SourceCompositionItem { Id = 1, Name = "Item 1"},
                    new SourceCompositionItem { Id = 2, Name = "Item 2"},
                    new SourceCompositionItem { Id = 3, Name = "Item 3"},
                }
            };

            MapperContext context = new MapperContext();

            var mapped = mapper.Map(source, target, context);

            Assert.Equal(mapped, mapped.CompositionList[0].Parent);
            Assert.Equal(mapped, mapped.CompositionList[1].Parent);
            Assert.Equal(mapped, mapped.CompositionList[2].Parent);
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
            public TargetEntity Parent { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Composition]
            public List<SourceCompositionItem> CompositionList { get; set; }
        }

        public class SourceCompositionItem
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}