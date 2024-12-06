﻿using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features
{
    public class MapEntityNullableKey
    {
        [Fact]
        public async Task map_entity_nullable_key()
        {
            var dbContext = await TestDbContext.Create<NullableKeyTestDbContext>();

            Customer customer = await dbContext.MapAsync<Customer>(new CustomerDto
            {
                Id = Guid.NewGuid(),
                Name = "new customer"
            });

            Assert.NotNull(customer);
            Assert.Equal("new customer", customer.Name);
        }

        [Fact]
        public async Task map_dto_nullable_key()
        {
            var dbContext = await TestDbContext.Create<NullableKeyTestDbContext>();

            Customer customer = await dbContext.MapAsync<Customer>(new CustomerDto
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

        public class CustomerDto
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