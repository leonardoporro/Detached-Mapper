using Detached.Annotations;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class InvoiceRow
    {
        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        public virtual double Quantity { get; set; }

        public virtual double Price { get; set; }

        [Composition]
        public virtual List<InvoiceRowDetail> RowDetails { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
