using Detached.Mappers.Samples.RestApi.Models.Core;
using System;

namespace Detached.Mappers.Samples.RestApi.Models.Outputs
{
    public class InvoiceListItem
    {
        public int Id { get; set; }
 
        public Item Type { get; set; }
 
        public Item Customer { get; set; }

        public DateTime DateTime { get; set; }
    }
}
