using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.ComplexModels
{
    public class Customer : OrganizationBase
    {
        public CustomerKindId CustomerKindId { get; set; } = CustomerKindId.Company;

        [Aggregation]
        [Required]
        public CustomerKind CustomerKind { get; set; }

        public string CustomerName { get; set; }

        [Composition]
        public List<Recommendation> Recommendations { get; set; }
    }
}
