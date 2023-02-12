namespace Detached.Mappers.EntityFramework.Tests.Model.DTOs
{
    public class InvoiceRowDTO
    {
        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        public virtual double Quantity { get; set; }

        public virtual double Price { get; set; }

        public byte[] RowVersion { get; set; }
    }
}