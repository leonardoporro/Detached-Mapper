using Detached.Mappers.EntityFramework.Tests.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Tests.Context
{
    public class TestDbContext : DbContext
    {
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

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceRow> InvoiceRows { get; set; }

        public DbSet<InvoiceType> InvoiceTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<SellPoint> SellPoints { get; set; }

        public DbSet<ConventionTestClass> ConventionTests { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>()
               .HasMany(u => u.Roles)
               .WithMany(r => r.Users)
               .UsingEntity<UserRole>(
                   ur => ur.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId),
                   ur => ur.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId))
               .HasKey(ur => new { ur.UserId, ur.RoleId });

            mb.Entity<ConventionTestClass>().HasKey(c => new { c.CustomizedKey1, c.CustomizedKey2 });

            mb.Entity<DeliveryArea>().HasDiscriminator(d => d.AreaType)
                .HasValue(typeof(CircleDeliveryArea), DeliveryAreaType.Circle)
                .HasValue(typeof(RectangleDeliveryArea), DeliveryAreaType.Rectangle);
        }

        public static async Task<DbContextOptions<TestDbContext>> CreateOptionsAsync([CallerMemberName] string dbName = null)
        {
            var connection = new SqliteConnection($"DataSource=file:{dbName}?mode=memory&cache=shared");

            await connection.OpenAsync();

            return new DbContextOptionsBuilder<TestDbContext>()
                    .UseSqlite(connection)
                    .UseDetached(cfg =>
                    {
                        ConventionTestClass.Configure(cfg);

                        cfg.Configure<DeliveryArea>().Discriminator(d => d.AreaType)
                                .Value(DeliveryAreaType.Circle, typeof(CircleDeliveryArea))
                                .Value(DeliveryAreaType.Rectangle, typeof(RectangleDeliveryArea));
                    })
                    .Options;
        }

        public static async Task<TestDbContext> CreateAsync([CallerMemberName] string dbName = null)
        {
            var context = new TestDbContext(await CreateOptionsAsync(dbName));
            await context.Database.EnsureCreatedAsync();
            return context;
        }
    }
}