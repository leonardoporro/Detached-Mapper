using EntityFrameworkCore.Detached.Plugins.Auditing;
using EntityFrameworkCore.Detached.Tests.Plugins.Auditing;
using EntityFrameworkCore.Detached.Tests.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Detached.Tests.Model
{
    public class TestDbContext : DbContext
    {
        public DbSet<Entity> Entities { get; set; }

        public DbSet<AssociatedReference> AssociatedReferences { get; set; }

        public DbSet<OwnedReference> OwnedReferences { get; set; }

        public DbSet<OwnedListItem> OwnedListItems { get; set; }

        public DbSet<AssociatedListItem> AssociatedListItems { get; set; }

        public DbSet<EntityForAuditing> EntitiesForAudit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var serviceProvider = new ServiceCollection()
                                         .AddEntityFrameworkInMemoryDatabase()
                                         .AddDetachedEntityFramework()
                                         .AddDetachedAuditing()
                                         .AddSingleton<ISessionInfoProvider>(SessionInfoProvider.Default)
                                         .BuildServiceProvider();

            optionsBuilder.UseInternalServiceProvider(serviceProvider)
                          .UseInMemoryDatabase()
                          .UseDetached(opts => opts.UseAuditing());
        }
    }
}