using Detached.Annotations;

namespace Detached.EntityFramework.Tests.Model.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }
    }
}