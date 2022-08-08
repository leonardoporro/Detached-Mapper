using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class OrganizationBaseDTO : IdBaseDTO
    {
        public string OrganizationType { get; set; }

        public string Name { get; set; }

        public int PrimaryAddressId { get; set; }
    }
}
