namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class OrganizationBaseDTO : IdBaseDTO
    {
        public string OrganizationType { get; set; }

        public string Name { get; set; }

        public int PrimaryAddressId { get; set; }
    }
}