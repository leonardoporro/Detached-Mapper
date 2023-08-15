using Detached.Annotations;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class OrganizationList : IdBase
    {
        public string ListName { get; set; }

        [Composition]
        public List<OrganizationBase> Organizations { get; set; } = new List<OrganizationBase>();
    }
}
