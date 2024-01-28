using Detached.Samples.RestApi.Models.Core;
using System;
using System.Collections.Generic;

namespace Detached.Samples.RestApi.Models.Inputs
{
    public class SaveInvoiceInput_Identity
    {
        public int Id { get; set; }

        public List<SaveInvoiceRowInput_Identity> Rows { get; set; }

        public Identity Type { get; set; }

        public Identity Customer { get; set; }

        public DateTime DateTime { get; set; }
    }
}