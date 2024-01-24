using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public class OrganizationBaseDto : IdBaseDto
    {
        public string OrganizationType { get; set; }

        public string Name { get; set; }

        public int PrimaryAddressId { get; set; }
    }
}
