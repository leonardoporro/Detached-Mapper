using Detached.Annotations;
using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features
{
    public class MapMemberWithValueConverter
    {
        [Fact]
        public async Task map_meber_with_value_converter()
        {
            var dbContext = await TestDbContext.Create<ValueConverterTestDbContext>();

            var address = await dbContext.MapAsync<Address>(new Address
            {
                Street = "Main St.",
                Number = "123",
                Tags = new List<Tag>
                 {
                     new Tag { Name = "primary" },
                     new Tag { Name = "local" }
                 }
            });

            await dbContext.SaveChangesAsync();


            await dbContext.MapAsync<Address>(new Address
            {
                Id = address.Id,
                Street = "Main St.",
                Number = "1234",
                Tags = new List<Tag>
                 {
                     new Tag { Name = "primary" },
                     new Tag { Name = "external" }
                 }
            });

            var persisted = dbContext.Set<Address>().FirstOrDefault(a => a.Id == address.Id);

            Assert.Equal("Main St.", persisted.Street);
            Assert.Equal("1234", persisted.Number);
            Assert.Contains(persisted.Tags, t => t.Name == "primary");
            Assert.Contains(persisted.Tags, t => t.Name == "external");
        }

        public class Address
        {
            [Key]
            public virtual int Id { get; set; }

            public virtual string Street { get; set; }

            public virtual string Number { get; set; }


            [Primitive]
            public List<Tag> Tags { get; set; }
        }

        public class Tag
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class TagListConverter : ValueConverter<List<Tag>, string>
        {
            public TagListConverter()
                : base(toProvider, fromProvider, null)
            {
            }

            static Expression<Func<List<Tag>, string>> toProvider = (tags) => string.Join(", ", tags.Select(t => t.Name));

            static Expression<Func<string, List<Tag>>> fromProvider = (tags) =>
                tags.Split(", ", StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => new Tag { Name = n })
                    .ToList();
        }

        public class ValueConverterTestDbContext : TestDbContext
        {
            public ValueConverterTestDbContext(DbContextOptions<ValueConverterTestDbContext> options)
                : base(options)
            {
            }

            public DbSet<Address> Addresses { get; set; }
        }
    }
}