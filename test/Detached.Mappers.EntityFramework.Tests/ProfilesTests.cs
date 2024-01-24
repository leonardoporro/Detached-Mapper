using Detached.Mappers.EntityFramework.Configuration;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class ProfilesTests
    {
        [Fact]
        public async Task multiple_profiles_create_profile()
        {
            var dbContext = await TestDbContext.Create<ProfilesTestDbContext>();

            UserDto dto = new UserDto { Id = 1, Name = "user name" };

            User newUser = dbContext.Map<User>(MappingProfiles.Create, dto);

            Assert.Equal(1, newUser.Id);
            Assert.Equal("user name", newUser.Name);
            Assert.Null(newUser.ModifiedDate);
            Assert.NotNull(newUser.CreatedDate);
        }

        [Fact]
        public async Task multiple_profiles_update_profile()
        {
            var dbContext = await TestDbContext.Create<ProfilesTestDbContext>();
            dbContext.Database.EnsureCreated();

            UserDto dto = new UserDto { Id = 1, Name = "user name" }; 

            User newUser = dbContext.Map<User>(MappingProfiles.Update, dto);

            Assert.Equal(1, newUser.Id);
            Assert.Equal("user name", newUser.Name);

            Assert.Null(newUser.CreatedDate);
            Assert.NotNull(newUser.ModifiedDate);
        }

        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime? ModifiedDate { get; set; }

            public DateTime? CreatedDate { get; set; }
        }

        public class UserDto
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public enum MappingProfiles
        {
            Create,
            Update
        }

        public class ProfilesTestDbContext : TestDbContext
        {
            public ProfilesTestDbContext(DbContextOptions<ProfilesTestDbContext> options) 
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }

            public override void OnMapperCreating(EntityMapperOptionsBuilder builder)
            {
                builder.AddProfile(MappingProfiles.Create, cfg =>
                {
                    cfg.Type<User>()
                       .FromType<UserDto>()
                       .Member(u => u.CreatedDate)
                       .FromValue((u, c) => (DateTime?)DateTime.Now);
                });

                builder.AddProfile(MappingProfiles.Update, cfg =>
                {
                    cfg.Type<User>()
                       .FromType<UserDto>()
                       .Member(u => u.ModifiedDate)
                       .FromValue((u, c) => (DateTime?)DateTime.Now);
                });
            }
        }
    }
}
