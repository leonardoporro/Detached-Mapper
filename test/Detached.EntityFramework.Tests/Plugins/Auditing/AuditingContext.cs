using Detached.EntityFramework.Plugins.Auditing;
using Detached.EntityFramework.Plugins.ManyToMany;
using Detached.EntityFramework.Plugins.PartialKey;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Tests.Plugins.Auditing
{
    public class AuditingContext : DbContext
    {
        public DbSet<EntityForAuditing> EntitiesForAuditing { get; set; }

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
                          .UseDetached();
        }
    }
}
