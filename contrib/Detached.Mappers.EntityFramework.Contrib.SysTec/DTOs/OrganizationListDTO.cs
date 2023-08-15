using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class OrganizationListDTO : IdBaseDTO
    {
        public string ListName { get; set; }

        public List<OrganizationBaseDTO> Organizations { get; set; } = new List<OrganizationBaseDTO>();
    }
}