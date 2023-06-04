using Detached.Annotations;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class Invoice
    {
        public virtual int Id { get; set; }

        [Aggregation]
        public virtual InvoiceType InvoiceType { get; set; }

        [Composition]
        public virtual List<InvoiceRow> Rows { get; set; }

        [Composition]
        public virtual ShippingAddress ShippingAddress { get; set; }
    }
}
