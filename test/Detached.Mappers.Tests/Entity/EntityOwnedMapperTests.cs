using Detached.Annotations;
using Detached.Mappers.Tests.Mocks;
using Detached.Mappers.TypeMappers.Entity;
using Xunit;

namespace Detached.Mappers.Tests.Entity
{
    public class EntityCompositionMapperTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void create_composition_entity_when_target_is_null()
        {
            // GIVEN models
            SourceDto source = new SourceDto
            {
                Id = 1,
                Name = "dto",
                Composition = new CompositionDto
                {
                    Id = 2,
                    Name = "composition_dto"
                }
            };

            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Composition = null
            };

            MapContextMock context = new MapContextMock();

            int targetCheck = target.GetHashCode();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);

            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty);
            Assert.NotNull(mapped.Composition);
            Assert.Equal(2, mapped.Composition.Id);
            Assert.Equal("composition_dto", mapped.Composition.Name);

            Assert.True(context.Verify<CompositionEntity>(new EntityKey<int>(2), out MapContextEntry entry));
            Assert.Equal(MapperActionType.Create, entry.ActionType);
        }

        [Fact]
        public void update_composition_entity_when_key_matches()
        {
            // GIVEN models
            SourceDto source = new SourceDto
            {
                Id = 1,
                Name = "dto",
                Composition = new CompositionDto
                {
                    Id = 2,
                    Name = "composition_dto"
                }
            };

            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Composition = new CompositionEntity
                {
                    Id = 2,
                    Name = "composition_entity"
                }
            };

            MapContextMock context = new MapContextMock();

            int targetCheck = target.GetHashCode();
            int compositionCheck = target.Composition.GetHashCode();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);

            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty);
            Assert.NotNull(mapped.Composition);
            Assert.Equal(compositionCheck, mapped.Composition.GetHashCode()); // composition is merged
            Assert.Equal(2, mapped.Composition.Id);
            Assert.Equal("composition_dto", mapped.Composition.Name);

            Assert.True(context.Verify<CompositionEntity>(new EntityKey<int>(2), out MapContextEntry entry));
            Assert.Equal(MapperActionType.Update, entry.ActionType);
        }

        [Fact]
        public void replace_composition_entity_when_key_not_matching()
        {
            // GIVEN models
            SourceDto source = new SourceDto
            {
                Id = 1,
                Name = "dto",
                Composition = new CompositionDto
                {
                    Id = 2,
                    Name = "composition_dto"
                }
            };

            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Composition = new CompositionEntity
                {
                    Id = 3,
                    Name = "composition_entity"
                }
            };

            int targetCheck = target.GetHashCode();
            int compositionCheck = target.Composition.GetHashCode();

            MapContextMock context = new MapContextMock();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);

            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty); // extra property is not overwritten

            Assert.NotNull(mapped.Composition);
            Assert.NotEqual(compositionCheck, target.Composition.GetHashCode()); // composition is replaced
            Assert.Equal(2, mapped.Composition.Id);
            Assert.Equal("composition_dto", mapped.Composition.Name);

            Assert.True(context.Verify<CompositionEntity>(new EntityKey<int>(2), out MapContextEntry createdEntry));
            Assert.Equal(MapperActionType.Create, createdEntry.ActionType);

            Assert.True(context.Verify<CompositionEntity>(new EntityKey<int>(3), out MapContextEntry deletedEntry));
            Assert.Equal(MapperActionType.Delete, deletedEntry.ActionType);
        }

        [Fact]
        public void delete_composition_entity_when_source_is_null()
        {
            // GIVEN model
            SourceDto source = new SourceDto
            {
                Id = 1,
                Name = "dto",
                Flag = true,
                Enum = TestEnum.Value1,
                Composition = null
            };

            TargetEntity target = new TargetEntity
            {
                Id = 1,
                Name = "entity",
                ExtraProperty = "extra_prop_not_mapped",
                Composition = new CompositionEntity
                {
                    Id = 3,
                    Name = "composition_entity"
                }
            };

            int targetCheck = target.GetHashCode();

            MapContextMock context = new MapContextMock();

            // WHEN mapped
            TargetEntity mapped = mapper.Map(source, target, context);

            // THEN
            Assert.Equal(targetCheck, mapped.GetHashCode()); // root is merged
            Assert.Equal(1, mapped.Id);
            Assert.Equal("dto", mapped.Name);
            Assert.Equal("extra_prop_not_mapped", mapped.ExtraProperty);
            Assert.Null(mapped.Composition); // composition is deleted

            Assert.True(context.Verify<CompositionEntity>(new EntityKey<int>(3), out MapContextEntry deletedEntry));
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
            public CompositionEntity Composition { get; set; }
        }

        [Entity]
        public class CompositionEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class SourceDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool Flag { get; set; }

            public TestEnum Enum { get; set; }

            public CompositionDto Composition { get; set; }
        }

        public class CompositionDto
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public enum TestEnum { Value1, Value2, Value3 }
    }
}