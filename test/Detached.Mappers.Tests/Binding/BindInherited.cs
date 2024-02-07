using Detached.Mappers.Exceptions;
using Xunit;

namespace Detached.Mappers.Tests.Binding
{
    public class BindInherited
    {
        [Fact]
        public void Bind_Inherited()
        {
            Mapper mapper = new Mapper();
            mapper.Options.Type<BaseDto>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteDto1))
                .HasValue(2, typeof(ConcreteDto2));

            mapper.Options.Type<BaseEntity>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteEntity1))
                .HasValue(2, typeof(ConcreteEntity2));
            
            var expression = mapper.Bind<BaseEntity, BaseDto>();

            var fn = expression.Compile();

            var dto = fn(new ConcreteEntity1
            {
                Id = 1,
                Name = "Concrete 1",
                Type = 1,
                ExtraProp1 = "Extra 1"
            });
 
            Assert.IsType<ConcreteDto1>(dto);
            Assert.Equal("Concrete 1", dto.Name);
            Assert.Equal("Extra 1", ((ConcreteDto1)dto).ExtraProp1);
        }

        [Fact]
        public void Bind_Inherited_MissingValue()
        {
            Mapper mapper = new Mapper();
            mapper.Options.Type<BaseDto>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteDto1));

            mapper.Options.Type<BaseEntity>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteEntity1))
                .HasValue(2, typeof(ConcreteEntity2));

            Assert.Throws<MapperException>(() => mapper.Bind<BaseEntity, BaseDto>());
        }


        [Fact]
        public void Bind_Inherited_InvalidDiscriminator()
        {
            Mapper mapper = new Mapper();
            mapper.Options.Type<BaseDto>()
                .Discriminator(b => b.Id)
                .HasValue(1, typeof(ConcreteDto1))
                .HasValue(2, typeof(ConcreteDto2));

            mapper.Options.Type<BaseEntity>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteEntity1))
                .HasValue(2, typeof(ConcreteEntity2));

            Assert.Throws<MapperException>(() => mapper.Bind<BaseEntity, BaseDto>());
        }


        public class BaseDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int Type { get; set; }
        }

        public class ConcreteDto1 : BaseDto
        {
            public string ExtraProp1 { get; set; }
        }

        public class ConcreteDto2 : BaseDto
        {
            public string ExtraProp2 { get; set; }
        }

        public class BaseEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int Type { get; set; }
        }

        public class ConcreteEntity1 : BaseEntity
        {
            public string ExtraProp1 { get; set; }
        }

        public class ConcreteEntity2 : BaseEntity
        {
            public string ExtraProp2 { get; set; }
        }
    }
}