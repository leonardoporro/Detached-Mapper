namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class CircleDeliveryArea : DeliveryArea
    {
        public CircleDeliveryArea()
        {
            AreaType = DeliveryAreaType.Circle;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Radius { get; set; }
    }
}
