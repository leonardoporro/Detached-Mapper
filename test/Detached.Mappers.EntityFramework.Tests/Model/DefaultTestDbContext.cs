using Detached.Mappers.EntityFramework.Tests.Model.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class DefaultTestDbContext : DbContext
    {
        public DefaultTestDbContext(DbContextOptions<DefaultTestDbContext> options)
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

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ConfiguredTestClass> ConventionTests { get; set; }
 
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>()
               .HasMany(u => u.Roles)
               .WithMany(r => r.Users)
               .UsingEntity<UserRole>(
                   ur => ur.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId),
                   ur => ur.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId))
               .HasKey(ur => new { ur.UserId, ur.RoleId });

            mb.Entity<ConfiguredTestClass>().HasKey(c => new { c.CustomizedKey1, c.CustomizedKey2 });

            mb.Entity<DeliveryArea>().HasDiscriminator(d => d.AreaType)
                .HasValue(typeof(CircleDeliveryArea), DeliveryAreaType.Circle)
                .HasValue(typeof(RectangleDeliveryArea), DeliveryAreaType.Rectangle);

            mb.Entity<Address>()
              .Property(a => a.Tags)
              .HasConversion(new TagListConverter());
        }

        public static async Task<DbContextOptions<DefaultTestDbContext>> CreateOptionsAsync([CallerMemberName] string dbName = null)
        {
            var connection = new SqliteConnection($"DataSource=file:{dbName}?mode=memory&cache=shared");

            await connection.OpenAsync();

            return new DbContextOptionsBuilder<DefaultTestDbContext>()
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .UseSqlite(connection)
                    .UseMapping(profiles =>
                    {
                        profiles.Default(config =>
                        {
                            config.Type<Invoice>()
                                  .Member(i => i.InvoiceType).Aggregation()
                                  .Member(i => i.Rows).Composition();

                            ConfiguredTestClass.Configure(config);
                        });
                    })
                    .Options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseDetached(mapperOptions => { });
        }

        public static async Task<DefaultTestDbContext> CreateAsync([CallerMemberName] string dbName = null)
        {
            var context = new DefaultTestDbContext(await CreateOptionsAsync(dbName));
            await context.Database.EnsureCreatedAsync();
            return context;
        }

    }
}