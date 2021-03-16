using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class Address
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string Street { get; set; }

        public virtual string Number { get; set; }
    }
}