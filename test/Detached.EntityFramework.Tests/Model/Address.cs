using Detached.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests.Model
{
    [Entity]
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }
    }
}