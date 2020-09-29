using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Samples.RestApi.Models
{
    [Entity]
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Composition]
        public List<InvoiceRow> Rows { get; set; }

        [Aggregation]
        public InvoiceType Type { get; set; }

        [Aggregation]
        public Customer Customer { get; set; }

        public DateTime DateTime { get; set; }
    }
}