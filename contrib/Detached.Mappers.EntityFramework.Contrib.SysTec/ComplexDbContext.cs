using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;
using Microsoft.Extensions.Logging;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DeepModel;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.inheritance;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec
{
    public class ComplexDbContext : DbContext
    {
        public DbSet<OrganizationList> OrganizationLists { get; set; }
        public DbSet<OrganizationBase> Organizations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Government> Governments { get; set; }
        public DbSet<SubGovernment> SubGovernments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<OrganizationNotes> OrganizationNotes { get; set; }
        public DbSet<CustomerKind> CustomerKinds { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<ReusedLinkedItem> ReusedLinkedItems { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<EntityOne> EntityOnes { get; set; }
        
        public DbSet<EntityTwo> EntityTwos { get; set; }
        
        public DbSet<EntityThree> EntityThrees { get; set; }
        
        public DbSet<EntityFour> EntityFours { get; set; }
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=ComplexDB.db;")
                .EnableSensitiveDataLogging()
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                .EnableDetailedErrors()
                .UseDetached(options =>
                {
                    // not what we want, because OrganizationBase can be Customer or Government
                    // but this line is a test to drive it further
                    //options.Configure<OrganizationBase>().Constructor(x => new Customer());

                    //options.Type<OrganizationBase>().Discriminator(o => o.OrganizationType)
                    //    .Value(nameof(Customer), typeof(Customer))
                    //    .Value(nameof(Government), typeof(Government))
                    //    .Value(nameof(SubGovernment), typeof(GovernmentLeader));

                    options.Type<OrganizationBaseDTO>().Abstract();

                    options.Type<OrganizationBaseDTO>()
                        .Discriminator(o => o.OrganizationType)
                        .HasValue<GovernmentDTO>(nameof(Government))
                        .HasValue<SubGovernmentDTO>(nameof(SubGovernment));

                    options.Type<BaseHead>()
                        .Discriminator(o => o.Discriminator)
                        .HasValue<EntityTwo>(nameof(EntityTwo))
                        .HasValue<EntityThree>(nameof(EntityThree))
                        .HasValue<EntityFour>(nameof(EntityFour))
                        .HasValue<EntityFive>(nameof(EntityFive));

                    /*
                     //Workaround for Test 15
                    options.Type<BaseStationOneSecond>()
                        .Discriminator(o => o.Discriminator)
                        .HasValue<EntityThree>(nameof(EntityThree));

                    options.Type<BaseStationOneFirst>()
                        .Discriminator(o => o.Discriminator)
                        .HasValue<EntityFour>(nameof(EntityFour))
                        .HasValue<EntityTwo>(nameof(EntityTwo)); */

                });

            //optionsBuilder.ConfigureWarnings(
            //    w => w.Ignore(CoreEventId.NavigationBaseIncludeIgnored)
            //);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationBase>()
                .HasDiscriminator(o => o.OrganizationType)
                .HasValue<Customer>(nameof(Customer))
                .HasValue<Government>(nameof(Government))
                .HasValue<SubGovernment>(nameof(SubGovernment));

            modelBuilder.Entity<OrganizationBase>()
                .HasOne<Address>(o => o.PrimaryAddress);

            modelBuilder.Entity<OrganizationBase>()
                .HasOne<Address>(o => o.ShipmentAddress);

            // Back-references don't work
            // Exception:
            // System.InvalidOperationException : An error was generated for warning
            // 'Microsoft.EntityFrameworkCore.Query.NavigationBaseIncludeIgnored': The navigation
            // 'OrganizationNotes.Organization' was ignored from 'Include' in the query since the fix-up
            // will automatically populate it. If any further navigations are specified in 'Include' afterwards
            // then they will be ignored. Walking back include tree is not allowed. This exception can be suppressed
            // or logged by passing event ID 'CoreEventId.NavigationBaseIncludeIgnored' to the 'ConfigureWarnings' method
            // in 'DbContext.OnConfiguring' or 'AddDbContext'.
            modelBuilder.Entity<OrganizationBase>()
                .HasMany<OrganizationNotes>(o => o.Notes);
            // .WithOne(n => n.Organization)
            // .HasForeignKey(k => k.OrganizationId);

            modelBuilder.Entity<OrganizationBase>(entity =>
            {
                entity.HasOne(s => s.Parent);
                entity.HasMany(s => s.Children)
                    .WithOne()
                    .HasForeignKey(s => s.ParentId);
            });

            modelBuilder.Entity<Customer>()
                .HasMany<Recommendation>(c => c.Recommendations)
                .WithOne(r => r.RecommendedBy);

            modelBuilder.Entity<BaseHead>()
                .UseTphMappingStrategy()
                .HasDiscriminator(b => b.Discriminator)
                .HasValue<EntityTwo>(nameof(EntityTwo))
                .HasValue<EntityThree>(nameof(EntityThree))
                .HasValue<EntityFour>(nameof(EntityFour));
        }

        public override int SaveChanges()
        {
            //var now = DateTime.Now;
            List<ConcurrencyStampBase> entities = new List<ConcurrencyStampBase>();
            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is ConcurrencyStampBase entity
                    && (changedEntity.State == EntityState.Added || changedEntity.State == EntityState.Modified))
                {
                    entities.Add(entity);
                }
            }

            try
            {
                int result = base.SaveChanges();


                if (entities.Count > 0)
                {
                    foreach (var entity in entities)
                    {
                        Entry(entity).Reload();
                        entity.ConcurrencyToken++;
                    }
                    base.SaveChanges();
                }

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entries = ex.Entries;
                throw;
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException;
                throw;
            }
        }
    }
}
