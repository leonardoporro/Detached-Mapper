using Detached.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Samples.RestApi.Models
{
    [Entity]
    public class InvoiceRow
    {
        [Key]
        public int Id { get; set; }

        [Aggregation]
        public StockUnit SKU { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public double Quantity { get; set; }
    }
}