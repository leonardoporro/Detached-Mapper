using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    [Collection("Default")]
    public class ProfilesTests
    {
        [Fact]
        public async Task multiple_profiles_create_profile()
        {
            ProfilesTestDbContext dbContext = await ProfilesTestDbContext.Create();

            UserDTO dto = new UserDTO { Id = 1, Name = "user name" };

            User newUser = dbContext.Map<User>(MappingProfiles.Create, dto);

            Assert.Equal(1, newUser.Id);
            Assert.Equal("user name", newUser.Name);
            Assert.Null(newUser.ModifiedDate);
            Assert.NotNull(newUser.CreatedDate);
        }

        [Fact]
        public async Task multiple_profiles_update_profile()
        {
            ProfilesTestDbContext dbContext = await ProfilesTestDbContext.Create();
            dbContext.Database.EnsureCreated();

            UserDTO dto = new UserDTO { Id = 1, Name = "user name" }; 

            User newUser = dbContext.Map<User>(MappingProfiles.Update, dto);

            Assert.Equal(1, newUser.Id);
            Assert.Equal("user name", newUser.Name);

            Assert.Null(newUser.CreatedDate);
            Assert.NotNull(newUser.ModifiedDate);
        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime? ModifiedDate { get; set; }

            public DateTime? CreatedDate { get; set; }
        }

        public class UserDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public enum MappingProfiles
        {
            Create,
            Update
        }

        public class ProfilesTestDbContext : DbContext
        {
            public ProfilesTestDbContext(DbContextOptions options) 
                : base(options)
            {
            }

            public static async Task<ProfilesTestDbContext> Create([CallerMemberName]string dbName = null)
            {
                var connection = new SQLiteConnection($"DataSource=file:{dbName}?mode=memory&cache=shared");
                await connection.OpenAsync();

                var options = new DbContextOptionsBuilder<ProfilesTestDbContext>()
                    .UseSqlite(connection)
                    .UseMapping(builder =>
                    {
                        builder.AddProfile(MappingProfiles.Create, cfg =>
                        {
                            cfg.Type<User>()
                               .FromType<UserDTO>()
                               .Member(u => u.CreatedDate)
                               .FromValue((u, c) => (DateTime?)DateTime.Now);
                        });

                        builder.AddProfile(MappingProfiles.Update, cfg =>
                        {
                            cfg.Type<User>()
                               .FromType<UserDTO>()
                               .Member(u => u.ModifiedDate)
                               .FromValue((u, c) => (DateTime?)DateTime.Now);
                        });
                    }).Options;

                var dbContext = new ProfilesTestDbContext(options);
                
                await dbContext.Database.EnsureCreatedAsync();

                return dbContext;
            }
        }
    }
}
