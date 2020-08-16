using Detached.Samples.RestApi.Models;
using Detached.Samples.RestApi.Models.Inputs;
using Detached.Samples.RestApi.Models.Outputs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Samples.RestApi.Stores
{
    public class InvoiceStore
    {
        readonly MainDbContext _context;

        public InvoiceStore(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceListItemOutput>> GetAsync()
        {
            return await _context.Project<Invoice, InvoiceListItemOutput>(_context.Invoices).ToListAsync();
        }

        public async Task SaveAsync(SaveInvoiceInput input)
        {
            await _context.MapAsync<Invoice>(input);
            await _context.SaveChangesAsync();
        }
    }
}