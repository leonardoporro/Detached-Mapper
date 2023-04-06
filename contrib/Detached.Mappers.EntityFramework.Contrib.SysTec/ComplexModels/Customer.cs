using Detached.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class Customer : OrganizationBase
    {
        public CustomerKindId? CustomerKindId { get; set; } = ComplexModels.CustomerKindId.Company;

        [Aggregation]
        [Required]
        public CustomerKind CustomerKind { get; set; }

        public string CustomerName { get; set; }

        [Composition]
        public List<Recommendation> Recommendations { get; set; }
    }
}
