using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using Detached.Mappers.EntityFramework.Tests.Model.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class MapOwnedTests
    {
        [Fact]
        public async Task map_owned()
        {
            TestDbContext db = await TestDbContext.CreateAsync();

            Invoice invoice = await db.MapAsync<Invoice>(new InvoiceDTO
            {
                ShippingAddress = new ShippingAddressDTO
                {
                    Line1 = "Zeballos St. 2135",
                    Line2 = "Suite 01",
                    Zip = "2000"
                }
            });

            await db.SaveChangesAsync();

            Invoice persisted = db.Invoices.Where(i => i.Id == invoice.Id).FirstOrDefault();
            Assert.NotNull(persisted);
            Assert.NotNull(persisted.ShippingAddress);
            Assert.Equal("Zeballos St. 2135", persisted.ShippingAddress.Line1);
            Assert.Equal("Suite 01", persisted.ShippingAddress.Line2);
            Assert.Equal("2000", persisted.ShippingAddress.Zip);
        }
    }
}
