using Detached.Mappers.Samples.RestApi.Models.Inputs;
using Detached.Mappers.Samples.RestApi.Models.Outputs;
using Detached.Mappers.Samples.RestApi.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Mappers.Samples.RestApi.Services
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

        public Task SaveAsync(SaveInvoiceInput input)
        {
            return _invoiceStore.SaveAsync(input);
        }
    }
}