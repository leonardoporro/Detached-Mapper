using Detached.Annotations;
using Detached.Mappers;
using Detached.Mappers.Context;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.Entity
{
    public class MayToManyParentTests
    {
        readonly Mapper mapper = new Mapper();

        [Fact]
        public void map_many_to_many_B_to_A()
        {
            EntityA source = new EntityA
            {
                Id = 1,
                Name = "root",
                ListOfB = new List<EntityB>
                {
                    new EntityB { Id = 1, Name = "First" },
                    new EntityB { Id = 2, Name = "Second" },
                }
            };

            EntityA target = null;


            MapperContext context = new MapperContext();

            var mapped = mapper.Map(source, target, context);

            Assert.Contains(mapped, mapped.ListOfB[0].ListOfA);
            Assert.Contains(mapped, mapped.ListOfB[1].ListOfA);
        }

        [Fact]
        public void map_many_to_many_A_to_B()
        {
            EntityB source = new EntityB
            {
                Id = 1,
                Name = "root",
                ListOfA = new List<EntityA>
                {
                    new EntityA { Id = 1, Name = "First" },
                    new EntityA { Id = 2, Name = "Second" },
                }
            };

            EntityB target = null;
 
            MapperContext context = new MapperContext();

            var mapped = mapper.Map(source, target, context);

            Assert.Contains(mapped, mapped.ListOfA[0].ListOfB);
            Assert.Contains(mapped, mapped.ListOfA[1].ListOfB);
        }

        [Entity]
        public class EntityA
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Composition]
            public List<EntityB> ListOfB { get; set; }
        }

        [Entity]
        public class EntityB
        {
            [Composition]
            public List<EntityA> ListOfA { get; set; } 

            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}