using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class ProfilesTests
    {
        [Fact]
        public void multiple_profiles_create_profile()
        {
            TestDbContext dbContext = new TestDbContext();
            dbContext.Database.EnsureCreated();

            UserDTO dto = new UserDTO  { Id = 1, Name = "user name" };

            User newUser = dbContext.Map<User>(MappingProfiles.Create, dto);

            Assert.Equal(1, newUser.Id);
            Assert.Equal("user name", newUser.Name);
            Assert.Null(newUser.ModifiedDate);
            Assert.NotNull(newUser.CreatedDate);
        }

        [Fact]
        public void multiple_profiles_update_profile()
        {
            TestDbContext dbContext = new TestDbContext();
            dbContext.Database.EnsureCreated();

            UserDTO dto = new UserDTO { Id = 1, Name = "user name" };

            User newUser = dbContext.Map<User>(MappingProfiles.Update, dto);

            Assert.Equal(1, newUser.Id);
            Assert.Equal("user name", newUser.Name);
           
            Assert.Null(newUser.CreatedDate);
            Assert.NotNull(newUser.ModifiedDate);
        }

        public class TestDbContext : DbContext
        {
            static SqliteConnection _connection;

            static TestDbContext()
            {
                _connection = new SqliteConnection($"DataSource=file:{Guid.NewGuid()}?mode=memory&cache=shared");
                _connection.Open();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(_connection)
                    .UseMapping(mapping =>
                    {
                        mapping.AddProfile(MappingProfiles.Create, cfg =>
                        {
                            cfg.Type<User>()
                               .FromType<UserDTO>()
                               .Member(u => u.CreatedDate)
                               .FromValue((u, c) => (DateTime?)DateTime.Now);
                        });

                        mapping.AddProfile(MappingProfiles.Update, cfg =>
                        {
                            cfg.Type<User>()
                               .FromType<UserDTO>()
                               .Member(u => u.ModifiedDate)
                               .FromValue((u, c) => (DateTime?)DateTime.Now);
                        });
                    });
            }

            public DbSet<User> Users { get; set; }
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
    }
}
