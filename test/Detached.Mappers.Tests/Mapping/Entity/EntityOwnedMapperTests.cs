using Detached.Annotations;
using Detached.Mappers;
using Detached.Mappers.Context;
using Detached.Mappers.TypeMaps;
using Xunit;

namespace Detached.Mappers.Tests.Mapping.Entity
{
    public class EntityOwnedMapperTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void create_owned_entity_when_target_is_null()
        {
            // GIVEN models
            SourceDTO source = new SourceDTO
            {
                Id = 1,
                Name = "dto",
                Owned = new OwnedDTO
                {
                    Id = 2,
                    Name = "owned_dto"
                }
            };

            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Owned = null
            };

            MapperContext context = new MapperContext();

            int targetCheck = target.GetHashCode();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);

            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty);
            Assert.NotNull(mapped.Owned);
            Assert.Equal(2, mapped.Owned.Id);
            Assert.Equal("owned_dto", mapped.Owned.Name);

            Assert.True(context.TryGetEntry<OwnedEntity>(new EntityKey<int>(2), out MapperContextEntry entry));
            Assert.Equal(MapperActionType.Create, entry.ActionType);
        }

        [Fact]
        public void update_owned_entity_when_key_matches()
        {
            // GIVEN models
            SourceDTO source = new SourceDTO
            {
                Id = 1,
                Name = "dto",
                Owned = new OwnedDTO
                {
                    Id = 2,
                    Name = "owned_dto"
                }
            };

            TargetEntity target = new TargetEntity
            { 
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Owned = new OwnedEntity
                {
                    Id = 2,
                    Name = "owned_entity"
                }
            };

            MapperContext context = new MapperContext();

            int targetCheck = target.GetHashCode();
            int ownedCheck = target.Owned.GetHashCode();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);
            
            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty);
            Assert.NotNull(mapped.Owned);
            Assert.Equal(ownedCheck, mapped.Owned.GetHashCode()); // owned is merged
            Assert.Equal(2, mapped.Owned.Id);
            Assert.Equal("owned_dto", mapped.Owned.Name);

            Assert.True(context.TryGetEntry<OwnedEntity>(new EntityKey<int>(2), out MapperContextEntry entry));
            Assert.Equal(MapperActionType.Update, entry.ActionType);
        }

        [Fact]
        public void replace_owned_entity_when_key_not_matching()
        {
            // GIVEN models
            SourceDTO source = new SourceDTO
            {
                Id = 1,
                Name = "dto",
                Owned = new OwnedDTO
                {
                    Id = 2,
                    Name = "owned_dto"
                }
            };

            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Owned = new OwnedEntity
                {
                    Id = 3,
                    Name = "owned_entity"
                }
            };

            int targetCheck = target.GetHashCode();
            int ownedCheck = target.Owned.GetHashCode();

            MapperContext context = new MapperContext();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);

            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty); // extra property is not overwritten

            Assert.NotNull(mapped.Owned);
            Assert.NotEqual(ownedCheck, target.Owned.GetHashCode()); // owned is replaced
            Assert.Equal(2, mapped.Owned.Id);
            Assert.Equal("owned_dto", mapped.Owned.Name);

            Assert.True(context.TryGetEntry<OwnedEntity>(new EntityKey<int>(2), out MapperContextEntry createdEntry));
            Assert.Equal(MapperActionType.Create, createdEntry.ActionType);

            Assert.True(context.TryGetEntry<OwnedEntity>(new EntityKey<int>(3), out MapperContextEntry deletedEntry));
            Assert.Equal(MapperActionType.Delete, deletedEntry.ActionType);
        }

        [Fact]
        public void delete_owned_entity_when_source_is_null()
        {
            // GIVEN model
            SourceDTO source = new SourceDTO
            {
                Id = 1,
                Name = "dto",
                Flag = true,
                Enum = TestEnum.Value1,
                Owned = null
            };

            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Owned = new OwnedEntity
                {
                    Id = 3,
                    Name = "owned_entity"
                }
            };

            int targetCheck = target.GetHashCode();

            MapperContext context = new MapperContext();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);

            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty);
            Assert.Null(mapped.Owned); // owned is deleted

            Assert.True(context.TryGetEntry<OwnedEntity>(new EntityKey<int>(3), out MapperContextEntry deletedEntry));
            Assert.Equal(MapperActionType.Delete, deletedEntry.ActionType);
        }

        [Entity]
        public class TargetEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string ExtraProperty { get; set; }

            public bool Flag { get; set; }

            public TestEnum Enum { get; set; }

            [Composition]
            public OwnedEntity Owned { get; set; }
        }

        [Entity]
        public class OwnedEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool Flag { get; set; }

            public TestEnum Enum { get; set; }

            public OwnedDTO Owned { get; set; }
        }

        public class OwnedDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public enum TestEnum { Value1, Value2, Value3 }
    }
}