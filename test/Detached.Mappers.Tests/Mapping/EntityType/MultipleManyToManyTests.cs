using Detached.Annotations;
using Detached.Mappers.Context;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.EntityType
{
    public class MultipleManyToManyTests
    {
        readonly Mapper mapper = new Mapper();

        [Fact]
        public void MapMultipleManyToMany()
        {
            EntityParent source = new() { Id = 1, Name = "ParentA" };
            EntityChild childA = new() { Id = 1, Name = "ChildA" };

            source.ListChildrenA = new();
            source.ListChildrenA.Add(childA);

            EntityParent target = new() { Id = 1 };

            MapperContext context = new MapperContext();

            var mapped = mapper.Map2(source, target, context);

            Assert.Single(source.ListChildrenA);
            Assert.Null(target.ListChildrenA[0].ListParentsB);
            Assert.Null(target.ListChildrenA[0].ListParentsC);
        }

        [Entity]
        public class EntityParent
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Aggregation]
            public List<EntityChild> ListChildrenA { get; set; }

            [Aggregation]
            public List<EntityChild> ListChildrenB { get; set; }

            [Aggregation]
            public List<EntityChild> ListChildrenC { get; set; }
        }

        [Entity]
        public class EntityChild
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public EntityParent BackReference { get; set; }

            [Aggregation]
            public List<EntityParent> ListParentsA { get; set; }

            [Aggregation]
            public List<EntityParent> ListParentsB { get; set; }

            [Aggregation]
            public List<EntityParent> ListParentsC { get; set; }
        }
    }
}