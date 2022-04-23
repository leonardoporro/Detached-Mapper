using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using Detached.Mappers.EntityFramework.Tests.Model.DTOs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class MapNullableKeyTests
    {
        [Fact]
        public async Task map_dto_with_nullable_key_value()
        {
            TestDbContext db = await TestDbContext.CreateAsync();

            Customer customer = await db.MapAsync<Customer>(new CustomerDTO
            {
                Id = Guid.NewGuid(),
                Name = "new customer"
            });

            Assert.NotNull(customer);
            Assert.Equal("new customer", customer.Name);
        }

        [Fact]
        public async Task map_dto_with_nullable_key_null()
        {
            TestDbContext db = await TestDbContext.CreateAsync();

            Customer customer = await db.MapAsync<Customer>(new CustomerDTO
            {
                Id = null,
                Name = "new customer"
            });

            Assert.NotNull(customer);
            Assert.Equal("new customer", customer.Name);
        }
    }
}