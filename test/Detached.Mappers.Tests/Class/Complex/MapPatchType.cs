using Detached.PatchTypes;
using Xunit;

namespace Detached.Mappers.Tests.Class.Complex
{
    public class MapPatchType
    {
        readonly Mapper _mapper = new Mapper();

        [Fact]
        public void map_patch_type()
        {
            Dto dto = (Dto)PatchTypeFactory.Create(typeof(Dto));
            dto.Id = 1;

            Entity entity = new Entity
            {
                Id = 5,
                Name = "test entity"
            };

            Entity result = _mapper.Map((IPatch)dto, entity);

            Assert.Equal(1, result.Id);
            Assert.Equal("test entity", result.Name);
        }
    }

    public class Dto
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
