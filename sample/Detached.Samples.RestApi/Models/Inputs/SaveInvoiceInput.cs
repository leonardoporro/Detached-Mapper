using System;
using System.Collections.Generic;

namespace Detached.Samples.RestApi.Models.Inputs
{
    public class SaveInvoiceInput
    {
        public int Id { get; set; }
 
        public List<SaveInvoiceRowInput> Rows { get; set; }

        public Identity Type { get; set; }

        public Identity Customer { get; set; }

        public DateTime DateTime { get; set; }
    }
}