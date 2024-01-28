using Detached.Samples.RestApi.Models.Inputs;
using Detached.Samples.RestApi.Models.Outputs;
using Detached.Samples.RestApi.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Samples.RestApi.Services
{
    public class InvoiceService
    {
        readonly InvoiceStore _invoiceStore;

        public InvoiceService(InvoiceStore invoiceStore)
        {
            _invoiceStore = invoiceStore;
        }

        public Task<IEnumerable<InvoiceListItem>> GetAsync()
        {
            return _invoiceStore.GetAsync();
        }

        public Task SaveAsync(SaveInvoiceInput_Identity input)
        {
            return _invoiceStore.SaveAsync(input);
        }

        public Task SaveAsync(SaveInvoiceInput_Primitive input)
        {
            return _invoiceStore.SaveAsync(input);
        }
    }
}