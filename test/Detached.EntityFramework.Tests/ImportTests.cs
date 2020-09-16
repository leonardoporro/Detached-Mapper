using Detached.EntityFramework.Tests.Context;
using Detached.EntityFramework.Tests.Model;
using Detached.Mapping;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.EntityFramework.Tests
{
    public class ImportTests
    {
        [Fact]
        public async Task map_json_string()
        {
            Mapper mapper = new Mapper();
            TestDbContext db = await TestDbContext.CreateInMemorySqliteAsync(mapper);

            await db.MapAsync<User>(new User 
            { 
                Id = 1, 
                Name = "test user",
                DateOfBirth = new DateTime(1984, 07, 09)
            });

            await db.SaveChangesAsync();

            await db.ImportJsonAsync<User>(@"[ { 'Id': 1, 'Name': 'test user 2' } ]".Replace("'", "\""));

            await db.SaveChangesAsync();
            
            User persisted = db.Users.Find(1);
            Assert.Equal("test user 2", persisted.Name);
            Assert.Equal(new DateTime(1984, 07, 09), persisted.DateOfBirth);
        }
    }
}
