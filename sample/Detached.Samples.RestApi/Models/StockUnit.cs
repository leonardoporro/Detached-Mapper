using Detached.Annotations;
using Detached.Mappers.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Detached.Samples.RestApi.Models
{
    [Entity]
    public class StockUnit
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}