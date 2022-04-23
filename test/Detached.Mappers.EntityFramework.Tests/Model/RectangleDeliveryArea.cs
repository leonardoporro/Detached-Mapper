namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class RectangleDeliveryArea : DeliveryArea
    {
        public RectangleDeliveryArea()
        {
            AreaType = DeliveryAreaType.Rectangle;
        }

        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }
    }
}
