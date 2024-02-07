using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Inherited
{
    public class MapInheritanceFluent
    {
        [Fact]
        public void map_inherited_using_discriminator()
        {
            List<object> animals = new List<object>
            {
                new { AnimalType = "cat", Id = 1, Name = "Snarf", HairType = HairType.Short },
                new { AnimalType = "fish", Id = 2, Name = "Nemo", HasTeeth = true },
            };

            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<Animal>()
                .Discriminator(a => a.AnimalType)
                .HasValue("cat", typeof(Cat))
                .HasValue("fish", typeof(Fish));

            Mapper mapper = new Mapper(mapperOptions);

            List<Animal> result = mapper.Map<List<object>, List<Animal>>(animals);

            Assert.IsType<Cat>(result[0]);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Snarf", result[0].Name);
            Assert.Equal(HairType.Short, ((Cat)result[0]).HairType);

            Assert.IsType<Fish>(result[1]);
            Assert.Equal(2, result[1].Id);
            Assert.Equal("Nemo", result[1].Name);
            Assert.True(((Fish)result[1]).HasTeeth);
        }


        public abstract class Animal
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string AnimalType { get; set; }
        }

        public class Fish : Animal
        {
            public bool HasTeeth { get; set; }
        }

        public class Cat : Animal
        {
            public HairType HairType { get; set; }
        }

        public enum HairType { Long, Short }
    }
}