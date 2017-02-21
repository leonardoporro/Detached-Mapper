using Detached.EntityFramework.Plugins.Auditing;
using Detached.EntityFramework.Tests.Plugins.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.EntityFramework.Tests
{
    public class TestDbContext : DbContext
    {
        public DbSet<Entity> Entities { get; set; }

        public DbSet<AssociatedReference> AssociatedReferences { get; set; }

        public DbSet<OwnedReference> OwnedReferences { get; set; }

        public DbSet<OwnedListItem> OwnedListItems { get; set; }

        public DbSet<AssociatedListItem> AssociatedListItems { get; set; }

        public DbSet<TwoReferencesSameTypeEntity> TwoReferencesEntity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var serviceProvider = new ServiceCollection()
                                         .AddEntityFrameworkInMemoryDatabase()
                                         .AddDetachedEntityFramework()
                                         .AddSingleton<ISessionInfoProvider>(SessionInfoProvider.Default)
                                         .BuildServiceProvider();

            optionsBuilder.UseInternalServiceProvider(serviceProvider)
                          .UseInMemoryDatabase()
                          .UseDetached(opts => opts.UseAuditing());
        }
    }
}