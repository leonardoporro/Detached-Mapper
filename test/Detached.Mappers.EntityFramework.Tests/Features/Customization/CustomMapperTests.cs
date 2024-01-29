using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features.Customization
{
    public class CustomMapperTests
    {
        [Fact]
        public async Task map_entity_to_id()
        {
            var dbContext = await TestDbContext.Create<CustomMapperDbContext>();

            var userDto = new UserDto
            {
                Id = 1,
                Name = "the user",
                Type = new UserTypeDto
                {
                    Id = 2,
                    Name = "System"
                }
            };

            var trackedUser = dbContext.Map<User>(userDto);

            Assert.NotNull(trackedUser);
            Assert.Equal(2, trackedUser.Type);
        }


        public class UserDto : IIdentity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public UserTypeDto Type { get; set; }
        }

        public class UserTypeDto : IIdentity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int Type { get; set; }
        }


        public class CustomMapperDbContext : TestDbContext
        {
            public CustomMapperDbContext(DbContextOptions<CustomMapperDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }

            public override void OnMapperCreating(EntityMapperOptions builder)
            {
                builder.Default(mapperOptions =>
                {
                    mapperOptions.TypeMapperFactories.Add(new EntityToIdMapperFactory());
                });
            }
        }

        public interface IIdentity
        {
            int Id { get; set; }
        }

        public class EntityToIdTypeMapper<TEntity> : TypeMapper<TEntity, int>
            where TEntity : IIdentity
        {
            public override int Map(TEntity source, int target, IMapContext context)
            {
                return source.Id;
            }
        }

        public class EntityToIdMapperFactory : ITypeMapperFactory
        {
            public bool CanCreate(Mapper mapper, TypePair typePair)
            {
                return typePair.SourceType.IsComplex() && typePair.TargetType.ClrType == typeof(int);
            }

            public ITypeMapper Create(Mapper mapper, TypePair typePair)
            {
                var mapperType = typeof(EntityToIdTypeMapper<>).MakeGenericType(typePair.SourceType.ClrType);
                return (ITypeMapper)Activator.CreateInstance(mapperType);
            }
        }
    }
}
