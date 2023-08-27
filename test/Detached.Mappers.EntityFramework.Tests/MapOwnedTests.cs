
using Detached.Annotations;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    [Collection("Default")]
    public class MapOwnedTests
    {
        [Fact]
        public async Task map_owned()
        {
            Invoice invoice;
            using (var dbContext = await TestDbContext.Create<OwnedTestDbContext>())
            {
                invoice = await dbContext.MapAsync<Invoice>(new InvoiceDTO
                {
                    Id = 1,
                    ShippingAddress = new ShippingAddressDTO
                    {
                        Line1 = "Zeballos St. 2135",
                        Line2 = "Suite 01",
                        Zip = "2000"
                    }
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = await TestDbContext.Create<OwnedTestDbContext>(false))
            {
                Invoice persisted = dbContext.Invoices.FirstOrDefault(i => i.Id == invoice.Id);
                Assert.NotNull(persisted);
                Assert.NotNull(persisted.ShippingAddress);
                Assert.Equal("Zeballos St. 2135", persisted.ShippingAddress.Line1);
                Assert.Equal("Suite 01", persisted.ShippingAddress.Line2);
                Assert.Equal("2000", persisted.ShippingAddress.Zip);
            }
        }

        public class Invoice
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            [Composition]
            public virtual ShippingAddress ShippingAddress { get; set; }
        }

        public class InvoiceDTO
        {
            public virtual int Id { get; set; }

            public virtual ShippingAddressDTO ShippingAddress { get; set; }
        }

        [Owned]
        public class ShippingAddress
        {
            public string Line1 { get; set; }

            public string Line2 { get; set; }

            public string Zip { get; set; }
        }

        public class ShippingAddressDTO
        {
            public string Line1 { get; set; }

            public string Line2 { get; set; }

            public string Zip { get; set; }
        }

        public class OwnedTestDbContext : TestDbContext
        {
            public OwnedTestDbContext(DbContextOptions<OwnedTestDbContext> options) 
                : base(options)
            {
            }

            public DbSet<Invoice> Invoices { get; set; }
        }
    }
}