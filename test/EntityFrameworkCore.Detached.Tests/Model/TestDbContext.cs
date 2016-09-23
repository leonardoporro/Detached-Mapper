using EntityFrameworkCore.Detached.Tests.Model.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Detached.Tests.Model
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
            : base(GetTestDbContextOptions())
        {

        }

        public DbSet<Entity> Entities { get; set; }

        public DbSet<AssociatedReference> AssociatedReferences { get; set; }

        public DbSet<OwnedReference> OwnedReferences { get; set; }

        public DbSet<OwnedListItem> OwnedListItems { get; set; }

        public DbSet<AssociatedListItem> AssociatedListItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        static DbContextOptions GetTestDbContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                     .AddEntityFrameworkInMemoryDatabase()
                     .AddEntityFrameworkDetached()
                     .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TestDbContext>();

            builder.UseInternalServiceProvider(serviceProvider)
                   .UseInMemoryDatabase();

            return builder.Options;
        }
    }
}