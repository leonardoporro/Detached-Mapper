using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Extensions;
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

        public async Task<IEnumerable<InvoiceListItem>> GetAsync()
        {
            return await _context.Project<Invoice, InvoiceListItem>(_context.Invoices).ToListAsync();
        }

        public async Task SaveAsync(SaveInvoiceInput_Identity input)
        {
            await _context.MapAsync<Invoice>(input);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(SaveInvoiceInput_Primitive input)
        {
            await _context.MapAsync<Invoice>(input);
            await _context.SaveChangesAsync();
        }
    }
}