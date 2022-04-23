namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public enum DeliveryAreaType
    {
        Rectangle,
        Circle
    }

    public abstract class DeliveryArea
    {
        public int Id { get; set; }

        public DeliveryAreaType AreaType { get; set; }
    }
}