using Detached.Annotations;
using Detached.Mappers;
using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Detached.Mappers.EntityFramework.Tests.Extensions.StringExtensions;

namespace Detached.Mappers.EntityFramework.Tests.Features
{
    public class ImportJson
    {
        [Fact]
        public async Task map_json_string()
        {
            var dbContext = await TestDbContext.Create<ImportTestDbContext>();

            await dbContext.MapAsync<User>(new User
            {
                Id = 1,
                Name = "test user",
                DateOfBirth = new DateTime(1984, 07, 09)
            });

            await dbContext.SaveChangesAsync();

            await dbContext.MapJsonAsync<User>(Json(@"[ { 'id': 1, 'name': 'test user 2' } ]"));

            await dbContext.SaveChangesAsync();

            User persisted = dbContext.Users.Find(1);
            Assert.Equal("test user 2", persisted.Name);
            Assert.Equal(new DateTime(1984, 07, 09), persisted.DateOfBirth);
        }

        [Fact]
        public async Task map_json_string_associations_ordered()
        {
            var dbContext = await TestDbContext.Create<ImportTestDbContext>();

            await dbContext.MapJsonAsync<Role>(
                Json(@"[
                            { 
                                'id': 1, 
                                'name': 'admin' 
                            }, 
                            {
                                'id': 2,
                                'name': 'power user'
                            }
                       ]"
                ));

            await dbContext.MapJsonAsync<User>(
                Json(@"[
                           { 
                               'id': 1, 
                               'name': 'test user',
                               'roles': [
                                    { 'id': 1 },
                                    { 'id': 2 }
                               ]
                           } 
                       ]"
                ));

            await dbContext.SaveChangesAsync();

            User user = dbContext.Users.Include(u => u.Roles).Where(u => u.Id == 1).FirstOrDefault();
            Assert.Equal(1, user.Id);
            Assert.Equal("test user", user.Name);
            Assert.Contains(user.Roles, u => u.Id == 1 && u.Name == "admin");
            Assert.Contains(user.Roles, u => u.Id == 2 && u.Name == "power user");
        }

        [Fact]
        public async Task map_json_string_associations_not_ordered()
        {
            var dbContext = await TestDbContext.Create<ImportTestDbContext>();

            await dbContext.MapJsonAsync<User>(
                 Json(@"[
                               { 
                                   'id': 1, 
                                   'name': 'test user',
                                   'roles': [
                                        { 'id': 1 },
                                        { 'id': 2 }
                                   ]
                               } 
                           ]"
                 ));

            await dbContext.MapJsonAsync<Role>(
                Json(@"[
                            { 
                                'id': 1, 
                                'name': 'admin' 
                            }, 
                            {
                                'id': 2,
                                'name': 'power user'
                            }
                       ]"
                ));

            await dbContext.SaveChangesAsync();

            User user = dbContext.Users.Include(u => u.Roles).Where(u => u.Id == 1).FirstOrDefault();
            Assert.Equal(1, user.Id);
            Assert.Equal("test user", user.Name);
            Assert.Contains(user.Roles, u => u.Id == 1 && u.Name == "admin");
            Assert.Contains(user.Roles, u => u.Id == 2 && u.Name == "power user");
        }

        [Fact]
        public async Task import_aggregation_and_compositions()
        {
            using (var dbContext = await TestDbContext.Create<ImportTestDbContext>())
            {
                // add some invoice types. aggregations are top level independent entities, and expected to be there 
                // when the root entity is mapped.
                dbContext.Add(new InvoiceType { Id = 1, Name = "A" });
                dbContext.Add(new InvoiceType { Id = 2, Name = "B" });
                await dbContext.SaveChangesAsync();

                // import from json.
                // only Id is needed for aggregations
                // undefined properties are not updated.
                string jsonCreate = Json(@"[{
				                            'id': 1,
				                            'invoiceType': { 'id': 2 },
				                            'rows': [
					                            {
                                                    'id': 1,
						                            'description': 'prod 1', 
						                            'quantity': 5,
						                            'price': 25
					                            },
					                            {
                                                    'id': 2,
						                            'description': 'prod 2',
						                            'quantity': 3,
						                            'price': 100
					                            }
				                            ]
			                            }]");

                // create an invoice by using Map method, passing an anonymous class.
                await dbContext.MapJsonAsync<Invoice>(jsonCreate);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = await TestDbContext.Create<ImportTestDbContext>(false))
            {
                Invoice persistedInvoice = dbContext.Invoices
                                                .Where(i => i.Id == 1)
                                                .Include(i => i.InvoiceType)
                                                .Include(i => i.Rows)
                                                .FirstOrDefault();

                // check that all created values are OK.
                Assert.NotNull(persistedInvoice);
                Assert.NotNull(persistedInvoice.InvoiceType);
                Assert.Equal("B", persistedInvoice.InvoiceType.Name);
                Assert.True(persistedInvoice.Rows.Count == 2);
                Assert.True(persistedInvoice.Rows.Any(r => r.Description == "prod 1" && r.Quantity == 5), "row values inserted");
                Assert.True(persistedInvoice.Rows.Any(r => r.Description == "prod 2" && r.Quantity == 3), "row values inserted");

                // update from json.
                // undefined properties are not updated!.
                string jsonUpdate = Json(@"[{
				                            'id': 1,
				                            'invoiceType': { 'id': 1 }
			                            }]");

                // create an invoice by using Map method, passing an anonymous class.
                await dbContext.MapJsonAsync<Invoice>(jsonUpdate);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = await TestDbContext.Create<ImportTestDbContext>(false))
            {
                Invoice updatedInvoice = dbContext.Invoices
                                                .Where(i => i.Id == 1)
                                                .Include(i => i.InvoiceType)
                                                .Include(i => i.Rows)
                                                .FirstOrDefault();

                // check that all created values are OK.
                Assert.NotNull(updatedInvoice);
                Assert.NotNull(updatedInvoice.InvoiceType);
                Assert.Equal("A", updatedInvoice.InvoiceType.Name);
                Assert.True(updatedInvoice.Rows.Count == 2);
                Assert.True(updatedInvoice.Rows.Any(r => r.Description == "prod 1" && r.Quantity == 5), "row values inserted");
                Assert.True(updatedInvoice.Rows.Any(r => r.Description == "prod 2" && r.Quantity == 3), "row values inserted");
            }
        }

        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime DateOfBirth { get; set; }

            public virtual List<Role> Roles { get; set; }

            [Composition]
            public virtual List<Address> Addresses { get; set; }

            public virtual UserType UserType { get; set; }

            [Composition]
            public virtual UserProfile Profile { get; set; }
        }

        public class UserProfile
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string FirstName { get; set; }

            public virtual string LastName { get; set; }
        }
        public class Role
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<User> Users { get; set; }
        }

        public class UserRole
        {
            public virtual int UserId { get; set; }

            public virtual User User { get; set; }

            public virtual int RoleId { get; set; }

            public virtual Role Role { get; set; }
        }

        public class UserType
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<User> Users { get; set; }
        }

        public class Address
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Street { get; set; }

            public virtual string Number { get; set; }
        }

        public class Invoice
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            [Aggregation]
            public virtual InvoiceType InvoiceType { get; set; }

            [Composition]
            public virtual List<InvoiceRow> Rows { get; set; }

            [Composition]
            public virtual ShippingAddress ShippingAddress { get; set; }
        }

        public class InvoiceRow
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }

            public string Description { get; set; }
        }

        public class InvoiceType
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }
        }

        [Owned]
        public class ShippingAddress
        {
            public string Line1 { get; set; }

            public string Line2 { get; set; }

            public string Zip { get; set; }
        }

        public class ImportTestDbContext : TestDbContext
        {
            public ImportTestDbContext(DbContextOptions<ImportTestDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }

            public DbSet<Invoice> Invoices { get; set; }

            protected override void OnModelCreating(ModelBuilder mb)
            {
                mb.Entity<User>()
                   .HasMany(u => u.Roles)
                   .WithMany(r => r.Users)
                   .UsingEntity<UserRole>(
                       ur => ur.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId),
                       ur => ur.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId))
                   .HasKey(ur => new { ur.UserId, ur.RoleId });
            }
        }
    }
}