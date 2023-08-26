using Detached.Annotations;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    [Collection("Default")]
    public class MapNestedTests
    {
        [Fact]
        public async Task map_two_levels()
        {
            var dbContext = await TestDbContext.Create<NestedTestDbContext>();

            dbContext.InvoiceTypes.Add(new InvoiceType() { Id = 1, });

            await dbContext.SaveChangesAsync();

            await dbContext.MapAsync<Invoice>(new Invoice
            {
                Id = 1,
                InvoiceType = new InvoiceType() { Id = 1 },
                Rows = new List<InvoiceRow> {
                    new InvoiceRow() {
                        Id = 1,
                        Description = "item desc.",
                        Price = 5,
                        Quantity = 2,
                        RowDetails = new List<InvoiceRowDetail> {
                            new InvoiceRowDetail { Id = 1, Description = "detail 1" },
                            new InvoiceRowDetail { Id = 2, Description = "detail 2" },
                            new InvoiceRowDetail { Id = 3, Description = "detail 3" },
                        }
                    },
                }
            });

            await dbContext.SaveChangesAsync();

            Invoice persistedInvoice = dbContext.Invoices
               .Include("Rows.RowDetails")
               .Where(i => i.Id == 1).FirstOrDefault();

            Assert.Equal(3, persistedInvoice.Rows[0].RowDetails.Count);
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 1");
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 2");
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 3");

            dbContext.ChangeTracker.Clear();

            await dbContext.MapAsync<Invoice>(new Invoice
            {
                Id = 1,
                InvoiceType = new InvoiceType() { Id = 1 },
                Rows = new List<InvoiceRow> {
                    new InvoiceRow() {
                        Id = 1,
                        Description = "item desc.",
                        Price = 5,
                        Quantity = 2,
                        RowDetails = new List<InvoiceRowDetail> {
                            new InvoiceRowDetail { Id = 1, Description = "detail 1" },
                            new InvoiceRowDetail { Id = 2, Description = "detail 2" },
                        }
                    },
                }
            });

            await dbContext.SaveChangesAsync();

            persistedInvoice = dbContext.Invoices
                .Include("Rows.RowDetails")
                .Where(i => i.Id == 1).FirstOrDefault();

            Assert.Equal(2, persistedInvoice.Rows[0].RowDetails.Count);
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 1");
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 2");
            Assert.DoesNotContain(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 3");
        }

        public class Invoice
        {
            public virtual int Id { get; set; }

            [Aggregation]
            public virtual InvoiceType InvoiceType { get; set; }

            [Composition]
            public virtual List<InvoiceRow> Rows { get; set; }
        }

        public class InvoiceRow
        {
            public virtual int Id { get; set; }

            public virtual string Description { get; set; }

            public virtual double Quantity { get; set; }

            public virtual double Price { get; set; }

            [Composition]
            public virtual List<InvoiceRowDetail> RowDetails { get; set; }

            public byte[] RowVersion { get; set; }

            [Parent]
            public Invoice Invoice { get; set; }
        }

        public class InvoiceRowDetail
        {
            public int Id { get; set; }

            public string Description { get; set; }
        }

        public class InvoiceType
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }
        }

        public class NestedTestDbContext : TestDbContext
        {
            protected NestedTestDbContext(DbContextOptions options) : base(options)
            {
            }

            public DbSet<Invoice> Invoices { get; set; }

            public DbSet<InvoiceType> InvoiceTypes { get; set; }
        }
    }
}
