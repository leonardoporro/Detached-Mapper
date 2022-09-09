using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class OrganizationListDTO : IdBaseDTO
    {
        public string ListName { get; set; }

        public List<OrganizationBaseDTO> Organizations { get; set; } = new List<OrganizationBaseDTO>();
    }
}
