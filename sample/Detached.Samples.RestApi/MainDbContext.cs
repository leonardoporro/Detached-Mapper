using Detached.Mappers.Samples.RestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.Samples.RestApi
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceRow> InvoiceRows { get; set; }

        public DbSet<InvoiceType> InvoiceTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<StockUnit> StockUnits { get; set; }
    }
}