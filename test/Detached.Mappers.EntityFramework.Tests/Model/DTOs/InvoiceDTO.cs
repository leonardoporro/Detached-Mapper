using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Tests.Model.DTOs
{
    public  class InvoiceDTO
    {
        public virtual int Id { get; set; }

        public virtual InvoiceTypeDTO InvoiceType { get; set; }

        public virtual List<InvoiceRowDTO> Rows { get; set; }

        public virtual ShippingAddressDTO ShippingAddress { get; set; }
    }
}