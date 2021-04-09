using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Detached.Mappers.EntityFramework.Tests.Extensions.StringExtensions;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class ImportTests
    {
        [Fact]
        public async Task map_json_string()
        {
            TestDbContext db = await TestDbContext.CreateAsync();

            await db.MapAsync<User>(new User
            {
                Id = 1,
                Name = "test user",
                DateOfBirth = new DateTime(1984, 07, 09)
            });

            await db.SaveChangesAsync();

            await db.MapJsonAsync<User>(Json(@"[ { 'id': 1, 'name': 'test user 2' } ]"));

            await db.SaveChangesAsync();

            User persisted = db.Users.Find(1);
            Assert.Equal("test user 2", persisted.Name);
            Assert.Equal(new DateTime(1984, 07, 09), persisted.DateOfBirth);
        }

        [Fact]
        public async Task map_json_string_associations_ordered()
        {
            TestDbContext db = await TestDbContext.CreateAsync();

            await db.MapJsonAsync<Role>(
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

            await db.MapJsonAsync<User>(
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

            await db.SaveChangesAsync();

            User user = db.Users.Include(u => u.Roles).Where(u => u.Id == 1).FirstOrDefault();
            Assert.Equal(1, user.Id);
            Assert.Equal("test user", user.Name);
            Assert.Contains(user.Roles, u => u.Id == 1 && u.Name == "admin");
            Assert.Contains(user.Roles, u => u.Id == 2 && u.Name == "power user");
        }

        [Fact]
        public async Task map_json_string_associations_not_ordered()
        {
            TestDbContext db = await TestDbContext.CreateAsync();

            await db.MapJsonAsync<User>(
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

            await db.MapJsonAsync<Role>(
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

            await db.SaveChangesAsync();

            User user = db.Users.Include(u => u.Roles).Where(u => u.Id == 1).FirstOrDefault();
            Assert.Equal(1, user.Id);
            Assert.Equal("test user", user.Name);
            Assert.Contains(user.Roles, u => u.Id == 1 && u.Name == "admin");
            Assert.Contains(user.Roles, u => u.Id == 2 && u.Name == "power user");
        }

        [Fact]
        public async Task import_aggregation_and_compositions()
        {
            var options = await TestDbContext.CreateOptionsAsync();

            using (TestDbContext context = new TestDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                // add some invoice types. aggregations are top level independent entities, and expected to be there 
                // when the root entity is mapped.
                context.Add(new InvoiceType { Id = 1, Name = "A" });
                context.Add(new InvoiceType { Id = 2, Name = "B" });
                await context.SaveChangesAsync();

                // import from json.
                // only Id is needed for aggregations
                // undefined properties are not updated.
                string jsonCreate = Json(@"[{
				                            'id': 1,
				                            'invoiceType': { 'id': 2 },
				                            'rows': [
					                            {
						                            'description': 'prod 1', 
						                            'quantity': 5,
						                            'price': 25
					                            },
					                            {
						                            'description': 'prod 2',
						                            'quantity': 3,
						                            'price': 100
					                            }
				                            ]
			                            }]");

                // create an invoice by using Map method, passing an anonymous class.
                await context.MapJsonAsync<Invoice>(jsonCreate);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(options))
            {
                Invoice persistedInvoice = context.Invoices
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
                await context.MapJsonAsync<Invoice>(jsonUpdate);
                await context.SaveChangesAsync();
            }

            using (TestDbContext context = new TestDbContext(options))
            {
                Invoice updatedInvoice = context.Invoices
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
    }
}