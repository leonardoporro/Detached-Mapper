using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Dictionary
{
    public class DictionaryTests
    {
        static Mapper mapper = new Mapper();

        [Fact]
        public void map_dictionary_to_entity()
        {
            var source = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "Name", "test name" },
                { "Associations", new List<Dictionary<string, object>>
                   {
                      new Dictionary<string, object>
                      {
                          { "Id", 1 },
                          { "Name", "asoc1" }
                      },
                      new Dictionary<string, object>
                      {
                          { "Id", 2 },
                          { "Name", "asoc2" }
                      },
                   }
                }
            };

            Entity result = mapper.Map<Dictionary<string, object>, Entity>(source);

            Assert.Equal(1, result.Id);
            Assert.Equal("test name", result.Name);
            Assert.NotNull(result.Associations);
            Assert.Equal(1, result.Associations[0].Id);
            Assert.Equal("asoc1", result.Associations[0].Name);
            Assert.Equal(2, result.Associations[1].Id);
            Assert.Equal("asoc2", result.Associations[1].Name);
        }

        [Fact]
        public void map_dictionary_as_patch()
        {
            Dictionary<string, object> patch = new Dictionary<string, object>
            {
                { "Name", "patched name!" }
            };

            Entity entity = new Entity { Id = 1, Name = "regular name" };


            Entity result = mapper.Map(patch, entity);

            Assert.Equal(1, result.Id);
            Assert.Equal("patched name!", result.Name);
        }

        public class Entity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<Association> Associations { get; set; }
        }

        public class Association
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

    }
}