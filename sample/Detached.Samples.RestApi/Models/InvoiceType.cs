using Detached.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Samples.RestApi.Models
{
    [Entity]
    public class InvoiceType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}