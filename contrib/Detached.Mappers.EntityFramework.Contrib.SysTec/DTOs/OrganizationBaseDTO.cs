using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class OrganizationBaseDTO : IdBaseDTO
    {
        [InheritanceDiscriminator(Value = nameof(Government), EntityType = typeof(Government), MappingType = typeof(GovernmentDTO))]
        [InheritanceDiscriminator(Value = nameof(SubGovernment), EntityType = typeof(SubGovernment), MappingType = typeof(SubGovernmentDTO))]
        public string OrganizationType { get; set; }

        public string Name { get; set; }

        public int PrimaryAddressId { get; set; }
    }
}
