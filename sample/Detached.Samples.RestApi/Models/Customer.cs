using Detached.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Samples.RestApi.Models
{
    [Entity]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DocumentNumber { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}