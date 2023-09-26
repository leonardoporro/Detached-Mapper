using Detached.Mappers.Exceptions;
using Xunit;

namespace Detached.Mappers.Tests.Binding
{
    public class BindInheritedTests
    {
        [Fact]
        public void Bind_Inherited()
        {
            Mapper mapper = new Mapper();
            mapper.Options.Type<BaseDTO>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteDTO1))
                .HasValue(2, typeof(ConcreteDTO2));

            mapper.Options.Type<BaseEntity>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteEntity1))
                .HasValue(2, typeof(ConcreteEntity2));
            
            var expression = mapper.Bind<BaseEntity, BaseDTO>();

            var fn = expression.Compile();

            var dto = fn(new ConcreteEntity1
            {
                Id = 1,
                Name = "Concrete 1",
                Type = 1,
                ExtraProp1 = "Extra 1"
            });
 
            Assert.IsType<ConcreteDTO1>(dto);
            Assert.Equal("Concrete 1", dto.Name);
            Assert.Equal("Extra 1", ((ConcreteDTO1)dto).ExtraProp1);
        }

        [Fact]
        public void Bind_Inherited_MissingValue()
        {
            Mapper mapper = new Mapper();
            mapper.Options.Type<BaseDTO>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteDTO1));

            mapper.Options.Type<BaseEntity>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteEntity1))
                .HasValue(2, typeof(ConcreteEntity2));

            Assert.Throws<MapperException>(() => mapper.Bind<BaseEntity, BaseDTO>());
        }


        [Fact]
        public void Bind_Inherited_InvalidDiscriminator()
        {
            Mapper mapper = new Mapper();
            mapper.Options.Type<BaseDTO>()
                .Discriminator(b => b.Id)
                .HasValue(1, typeof(ConcreteDTO1))
                .HasValue(2, typeof(ConcreteDTO2));

            mapper.Options.Type<BaseEntity>()
                .Discriminator(b => b.Type)
                .HasValue(1, typeof(ConcreteEntity1))
                .HasValue(2, typeof(ConcreteEntity2));

            Assert.Throws<MapperException>(() => mapper.Bind<BaseEntity, BaseDTO>());
        }


        public class BaseDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int Type { get; set; }
        }

        public class ConcreteDTO1 : BaseDTO
        {
            public string ExtraProp1 { get; set; }
        }

        public class ConcreteDTO2 : BaseDTO
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