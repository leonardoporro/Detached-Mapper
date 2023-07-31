using Detached.Annotations;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Detached.Mappers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using static Detached.Mappers.EntityFramework.Tests.ProfilesTests;

namespace Detached.Mappers.EntityFramework.Tests
{
    [Collection("Default")]
    public class MapInheritanceTests
    {
        [Fact]
        public async Task map_inherited_entities_success()
        {
            InheritanceTestDbContext dbContext = await InheritanceTestDbContext.Create();

            SellPoint sellPoint = dbContext.Map<SellPoint>(new
            {
                Id = 1,
                DeliveryAreas = new object[]
                {
                    new { Id = 1, AreaType = DeliveryAreaType.Circle, X = 0.25, Y = 0.25, Radius = 10.0 },
                    new { Id = 2, AreaType = DeliveryAreaType.Rectangle, X1 = 0.25, Y1 = 0.25, X2 = 1, Y2 = 1 }
                }
            });

            Assert.NotNull(sellPoint);
            Assert.NotNull(sellPoint.DeliveryAreas);
            Assert.Equal(2, sellPoint.DeliveryAreas.Count);
            Assert.IsType<CircleDeliveryArea>(sellPoint.DeliveryAreas[0]);
            Assert.IsType<RectangleDeliveryArea>(sellPoint.DeliveryAreas[1]);

            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task map_inherited_entities_invalid_discriminator()
        {
            InheritanceTestDbContext dbContext = await InheritanceTestDbContext.Create();

            try
            {
                SellPoint sellPoint = dbContext.Map<SellPoint>(new
                {
                    Id = 1,
                    DeliveryAreas = new object[]
                    {
                        new { Id = 1, AreaType = (DeliveryAreaType)50, X = 0.25, Y = 0.25, Radius = 10.0 }
                    }
                });

                throw new Exception("Test is not working!");
            }
            catch (Exception x)
            {
                Assert.IsType<MapperException>(x);
                Assert.Equal($"50 is not a valid value for {typeof(DeliveryArea).FullName} discriminator.", x.Message);
            }

            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task map_inherited_entities_missing_discriminator()
        {
            InheritanceTestDbContext dbContext = await InheritanceTestDbContext.Create();

            try
            {
                SellPoint sellPoint = dbContext.Map<SellPoint>(new
                {
                    Id = 1,
                    DeliveryAreas = new object[]
                    {
                        new { Id = 1, X = 0.25, Y = 0.25, Radius = 10.0 }
                    }
                });

                throw new Exception("Test is not working!");
            }
            catch (Exception x)
            {
                Assert.IsType<MapperException>(x);
            }

            await dbContext.SaveChangesAsync();
        }

        public class SellPoint
        {
            public int Id { get; set; }

            [Composition]
            public List<DeliveryArea> DeliveryAreas { get; set; }
        }

        public enum DeliveryAreaType
        {
            Rectangle,
            Circle
        }

        public abstract class DeliveryArea
        {
            public int Id { get; set; }

            public DeliveryAreaType AreaType { get; set; }
        }

        public class RectangleDeliveryArea : DeliveryArea
        {
            public RectangleDeliveryArea()
            {
                AreaType = DeliveryAreaType.Rectangle;
            }

            public double X1 { get; set; }

            public double Y1 { get; set; }

            public double X2 { get; set; }

            public double Y2 { get; set; }
        }

        public class CircleDeliveryArea : DeliveryArea
        {
            public CircleDeliveryArea()
            {
                AreaType = DeliveryAreaType.Circle;
            }

            public double X { get; set; }

            public double Y { get; set; }

            public double Radius { get; set; }
        }

        public class InheritanceTestDbContext : DbContext
        {
            public InheritanceTestDbContext(DbContextOptions options)
                : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder mb)
            {
                mb.Entity<DeliveryArea>().HasDiscriminator(d => d.AreaType)
                    .HasValue(typeof(CircleDeliveryArea), DeliveryAreaType.Circle)
                    .HasValue(typeof(RectangleDeliveryArea), DeliveryAreaType.Rectangle);
            }

            public static async Task<InheritanceTestDbContext> Create([CallerMemberName] string dbName = null)
            {
                var connection = new SQLiteConnection($"DataSource=file:{dbName}?mode=memory&cache=shared");
                await connection.OpenAsync();

                var options = new DbContextOptionsBuilder<InheritanceTestDbContext>()
                    .UseSqlite(connection)
                    .UseMapping()
                    .Options;

                var dbContext = new InheritanceTestDbContext(options);

                await dbContext.Database.EnsureCreatedAsync();

                return dbContext;
            }
        }
    }
}