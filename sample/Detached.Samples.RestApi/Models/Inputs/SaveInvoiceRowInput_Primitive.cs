namespace Detached.Samples.RestApi.Models.Inputs
{
    public class SaveInvoiceRowInput_Primitive
    {
        public int Id { get; set; }

        public int SKUId { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public double Quantity { get; set; }
    }
}