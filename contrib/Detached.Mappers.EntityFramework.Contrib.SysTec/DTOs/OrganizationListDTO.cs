using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public class OrganizationListDto : IdBaseDto
    {
        public string ListName { get; set; }

        public List<OrganizationBaseDto> Organizations { get; set; } = new List<OrganizationBaseDto>();
    }
}