using Detached.PatchTypes;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Complex
{
    public class PatchTests
    {
        readonly Mapper _mapper = new Mapper();

        [Fact]
        public void map_ipatch_type()
        {
            DTO dto = (DTO)PatchTypeFactory.Create(typeof(DTO));
            dto.Id = 1;

            Entity entity = new Entity
            {
                Id = 5,
                Name = "test entity"
            };

            Entity result = _mapper.Map2((IPatch)dto, entity);

            Assert.Equal(1, result.Id);
            Assert.Equal("test entity", result.Name);
        }
    }

    public class DTO
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
    }

    public class Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
