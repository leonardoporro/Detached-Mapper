using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    [Collection("Default")]
    public class MapNullableKeyTests
    {
        [Fact]
        public async Task map_dto_with_nullable_key_value()
        {
            NullableKeyTestDbContext dbContext = new();

            Customer customer = await dbContext.MapAsync<Customer>(new CustomerDTO
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
            NullableKeyTestDbContext dbContext = new();

            Customer customer = await dbContext.MapAsync<Customer>(new CustomerDTO
            {
                Id = null,
                Name = "new customer"
            });

            Assert.NotNull(customer);
            Assert.Equal("new customer", customer.Name);
        }

        public class Customer
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }

        public class CustomerDTO
        {
            public Guid? Id { get; set; }

            public string Name { get; set; }
        }

        public class NullableKeyTestDbContext : TestDbContext
        {
            public DbSet<Customer> Customers { get; set; }
        }
    }
}