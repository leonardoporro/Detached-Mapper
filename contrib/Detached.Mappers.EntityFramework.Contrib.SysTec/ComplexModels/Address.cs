using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class Address : IdBase
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        
        public int CountryId { get; set; }
        [Aggregation]
        public Country Country { get; set; }
    }
}