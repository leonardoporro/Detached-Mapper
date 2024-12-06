using Xunit;

namespace Detached.Mappers.Tests.Class.Abstract
{
    public class MapAbstractBoxedValue
    {
        [Fact]
        public void map_abstract_boxedtype()
        {
            Mapper mapper = new Mapper();

            object source = new SourceType
            {
                Id = 1,
                Name = "BoxedSourceType"
            };

            TargetType result = mapper.Map(source, typeof(SourceType), null, typeof(TargetType), null) as TargetType;

            Assert.NotNull(result);
            Assert.Equal("BoxedSourceType", result.Name);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void map_abstract_boxedmembers()
        {
            Mapper mapper = new Mapper();

            RootType source = new RootType
            {
                Value = new SourceType
                {
                    Id = 1,
                    Name = "BoxedSourceType"
                }
            };

            RootType target = new RootType
            {
                Value = new TargetType
                {
                    Id = 2,
                    Name = "BoxedTargetType"
                }
            };

            RootType result = mapper.Map(source, target);

            Assert.NotNull(result);
            Assert.Equal("BoxedSourceType", ((TargetType)result.Value).Name);
            Assert.Equal(1, ((TargetType)result.Value).Id);
        }

        public class SourceType
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class TargetType
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class RootType
        {
            public object Value { get; set; }
        }
    }
}