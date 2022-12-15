using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Detached.Mappers.Annotations;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class ProfilesTests
    {
        [Fact]
        public void multiple_profiles_should_map_accordingly()
        {
            TestDbContext dbContext = new TestDbContext();
            dbContext.Database.EnsureCreated();

            var dto = new { Id = 1, Name = "user name", DateOfBirth = DateTime.Now };

            User nameOnlyUser = dbContext.Map<User>(MappingProfiles.NameOnly, dto);

            Assert.Null(nameOnlyUser.DateOfBirth);

            User allUser = dbContext.Map<User>(MappingProfiles.All, dto);

            Assert.NotNull(allUser.DateOfBirth);
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
                        mapping.AddProfile(MappingProfiles.NameOnly, cfg =>
                        {
                            cfg.Type<User>().Member(u => u.DateOfBirth).NotMapped();
                        });

                        mapping.AddProfile(MappingProfiles.All);
                    });
            }

            public DbSet<User> Users { get; set; }
        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime? DateOfBirth { get; set; }
        }

        public enum MappingProfiles
        {
            All,
            NameOnly
        }
    }
}
