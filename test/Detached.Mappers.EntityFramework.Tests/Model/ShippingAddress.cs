using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    [Owned]
    public class ShippingAddress
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Zip { get; set; }
    }
}
