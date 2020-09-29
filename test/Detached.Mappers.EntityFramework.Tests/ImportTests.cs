using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using System;
using System.Data.Entity;
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
            TestDbContext db = await TestDbContext.CreateInMemorySqliteAsync();

            await db.MapAsync<User>(new User
            {
                Id = 1,
                Name = "test user",
                DateOfBirth = new DateTime(1984, 07, 09)
            });

            await db.SaveChangesAsync();

            await db.ImportJsonAsync<User>(Json(@"[ { 'id': 1, 'name': 'test user 2' } ]"));

            await db.SaveChangesAsync();

            User persisted = db.Users.Find(1);
            Assert.Equal("test user 2", persisted.Name);
            Assert.Equal(new DateTime(1984, 07, 09), persisted.DateOfBirth);
        }

        [Fact]
        public async Task map_json_string_associations_ordered()
        {
            TestDbContext db = await TestDbContext.CreateInMemorySqliteAsync();

            await db.ImportJsonAsync<Role>(
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

            await db.ImportJsonAsync<User>(
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
            TestDbContext db = await TestDbContext.CreateInMemorySqliteAsync();

            await db.ImportJsonAsync<User>(
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

            await db.ImportJsonAsync<Role>(
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
    }
}