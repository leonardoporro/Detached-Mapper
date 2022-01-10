using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class MapTwoLevels
    {
        [Fact]
        public async Task MapTwoLevelsAsync()
        {
            TestDbContext db = await TestDbContext.CreateAsync();
            db.InvoiceTypes.Add(new InvoiceType() { Id = 1, });
            await db.SaveChangesAsync();

            await db.MapAsync<Invoice>(new Invoice
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
            
            await db.SaveChangesAsync();

            Invoice persistedInvoice = db.Invoices
               .Include("Rows.RowDetails")
               .Where(i => i.Id == 1).FirstOrDefault();

            Assert.Equal(3, persistedInvoice.Rows[0].RowDetails.Count);
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 1");
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 2");
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 3");
 
            await db.MapAsync<Invoice>(new Invoice
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

            persistedInvoice = db.Invoices
                .Include("Rows.RowDetails")
                .Where(i => i.Id == 1).FirstOrDefault();

            Assert.Equal(2, persistedInvoice.Rows[0].RowDetails.Count);
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 1");
            Assert.Contains(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 2");
            Assert.DoesNotContain(persistedInvoice.Rows[0].RowDetails, r => r.Description == "detail 3");
        }
    }
}
