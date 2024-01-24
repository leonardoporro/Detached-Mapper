using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public class OrganizationListDto : IdBaseDto
    {
        public string ListName { get; set; }

        public List<OrganizationBaseDto> Organizations { get; set; } = new List<OrganizationBaseDto>();
    }
}
