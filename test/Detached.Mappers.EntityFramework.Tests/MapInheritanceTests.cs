using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using Detached.Mappers.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class MapInheritanceTests
    {
        [Fact]
        public async Task map_inherited_entities_success()
        {
            TestDbContext dbContext = await TestDbContext.CreateAsync();

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
            TestDbContext dbContext = await TestDbContext.CreateAsync();

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
                Assert.Equal("50 is not a valid value for discriminator in entity Detached.Mappers.EntityFramework.Tests.Model.DeliveryArea.", x.Message);
            }

            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task map_inherited_entities_missing_discriminator()
        {
            TestDbContext dbContext = await TestDbContext.CreateAsync();

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
                Assert.Equal("Entity Detached.Mappers.EntityFramework.Tests.Model.DeliveryArea uses inheritance but discriminator was not found in source type.", x.Message);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
