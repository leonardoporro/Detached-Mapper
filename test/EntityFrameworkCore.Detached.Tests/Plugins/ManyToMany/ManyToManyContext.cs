using EntityFrameworkCore.Detached.Plugins.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Detached.Tests.Plugins.ManyToMany
{
    public class ManyToManyContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var serviceProvider = new ServiceCollection()
                                         .AddEntityFrameworkInMemoryDatabase()
                                         .AddDetachedEntityFramework()
                                         .AddManyToManyHelpers()
                                         .BuildServiceProvider();

            optionsBuilder.UseInternalServiceProvider(serviceProvider)
                          .UseInMemoryDatabase()
                          .UseDetached();
        }
    }
}
