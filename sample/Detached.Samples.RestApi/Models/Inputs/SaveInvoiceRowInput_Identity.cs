﻿using Detached.Samples.RestApi.Models.Core;

namespace Detached.Samples.RestApi.Models.Inputs
{
    public class SaveInvoiceRowInput_Identity
    {
        public int Id { get; set; }

        public Identity SKU { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public double Quantity { get; set; }
    }
}