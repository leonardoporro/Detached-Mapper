using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
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

        public int? NewAddressId { get; set; }

        [Composition]
        public Address NewAddress { get; set; }

        [Composition]
        public List<Tag> Tags { get; set; }// = new List<Tag>();

        [Composition]
        public List<OrganizationNotes> Notes { get; set; } = new List<OrganizationNotes>();

        public int? ParentId { get; set; }
        
        [Parent]
        public OrganizationBase Parent { get; set; }

        [Aggregation]
        public List<OrganizationBase> Children { get; set; } = new List<OrganizationBase>();
    }
}