using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.ComplexModels
{
    public abstract class OrganizationBase : IdBase
    {
        public string OrganizationType { get; set; }

        public string Name { get; set; }

        public int PrimaryAddressId { get; set; }
        [Aggregation]
        public Address PrimaryAddress { get; set; }

        public int? ShipmentAddressId { get; set; }
        [Aggregation]
        public Address ShipmentAddress { get; set; }

        [Composition]
        public List<Tag> Tags { get; set; } = new List<Tag>();

        [Composition]
        public List<OrganizationNotes> Notes { get; set; } = new List<OrganizationNotes>();

        public int? ParentId { get; set; }
        [Aggregation]
        public OrganizationBase Parent { get; set; }
        [Aggregation]
        public List<OrganizationBase> Children { get; set; } = new List<OrganizationBase>();
    }
}