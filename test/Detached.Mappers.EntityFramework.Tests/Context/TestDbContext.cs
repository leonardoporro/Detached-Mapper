using Detached.Mappers.EntityFramework.Tests.Model;
using Detached.Mappers.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Tests.Context
{
    public class TestDbContext : DbContext
    {
        readonly IList<IDisposable> _resources = new List<IDisposable>();

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<Address> Addresses { get; set; }

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

        public void OnMapperCreating(MapperOptions options)
        {

        }

        public override void Dispose()
        {
            foreach (IDisposable resource in _resources)
            {
                resource.Dispose();
            }

            base.Dispose();
        }

        public static async Task<TestDbContext> CreateInMemorySqliteAsync()
        {
            var connection = new SqliteConnection($"DataSource=file:{Guid.NewGuid()}?mode=memory&cache=shared");

            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<TestDbContext>()
                    .UseSqlite(connection)
                    .UseDetached()
                    .Options;

            var context = new TestDbContext(options);
            context._resources.Add(connection);

            await context.Database.EnsureCreatedAsync();

            return context;
        }
    }
}