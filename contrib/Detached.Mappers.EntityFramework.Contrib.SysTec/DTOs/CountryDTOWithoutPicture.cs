namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class CountryDTOWithoutPicture : IdBaseDTO
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public int? FlagPictureId { get; set; }
    }
}
