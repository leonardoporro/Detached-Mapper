namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class Country : IdBase
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public int? FlagPictureId { get; set; }
        public Picture FlagPicture { get; set; }
    }
}