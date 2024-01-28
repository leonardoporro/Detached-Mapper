using System;
using System.Collections.Generic;

namespace Detached.Samples.RestApi.Models.Inputs
{
    public class SaveInvoiceInput_Primitive
    {
        public int Id { get; set; }

        public List<SaveInvoiceRowInput_Primitive> Rows { get; set; }

        public int TypeId { get; set; }

        public int CustomerId { get; set; }

        public DateTime DateTime { get; set; }
    }
}