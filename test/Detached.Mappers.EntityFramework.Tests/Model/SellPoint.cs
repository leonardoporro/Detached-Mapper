using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class SellPoint
    {
        public int Id { get; set; }

        public List<DeliveryArea> DeliveryAreas { get; set; }
    }
}