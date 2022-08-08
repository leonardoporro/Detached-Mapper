using GraphInheritenceTests.ComplexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class OrganizationList : IdBase
    {
        public string ListName { get; set; }

        public List<OrganizationBase> Organizations { get; set; } = new List<OrganizationBase>();
    }
}
