using Detached.EntityFramework;
using Detached.EntityFramework.Queries;
using Detached.Mapping;
using Detached.Samples.RestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Detached.Samples.RestApi
{
    public class MainDbContext : DetachedDbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options, Mapper mapper, QueryProvider queryProvider) 
            : base(options, mapper, queryProvider)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceRow> InvoiceRows { get; set; }

        public DbSet<InvoiceType> InvoiceTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<StockUnit> StockUnits { get; set; }
    }
}