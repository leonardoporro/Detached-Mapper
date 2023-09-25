using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
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
            var dbContext = await TestDbContext.Create<NullableKeyTestDbContext>();

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
            var dbContext = await TestDbContext.Create<NullableKeyTestDbContext>();

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
            public NullableKeyTestDbContext(DbContextOptions<NullableKeyTestDbContext> options) 
                : base(options)
            {
            }

            public DbSet<Customer> Customers { get; set; }
        }
    }
}