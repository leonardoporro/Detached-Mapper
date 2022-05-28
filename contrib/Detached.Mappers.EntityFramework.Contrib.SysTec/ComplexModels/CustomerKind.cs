using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.ComplexModels
{
    public class CustomerKind
    {
        [Key]
        public CustomerKindId Id { get; set; }

        public string Name { get; set; }

        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}