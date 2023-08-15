using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs;

namespace GraphInheritenceTests.DTOs
{
    public class CountryDTO : IdBaseDTO
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public int? FlagPictureId { get; set; }
        public PictureDTO FlagPicture { get; set; }
    }
}
