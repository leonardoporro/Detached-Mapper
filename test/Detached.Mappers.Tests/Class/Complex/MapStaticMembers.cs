using Detached.Mappers.Types;
using Xunit;

namespace Detached.Mappers.Tests.Class.Complex
{
    // https://github.com/leonardoporro/Detached-Mapper/issues/126
    public class MapStaticMembers
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void static_properties_are_not_members()
        {
            IType type = mapper.Options.GetType(typeof(EntityWithStatics));

            Assert.DoesNotContain(nameof(EntityWithStatics.Default), type.MemberNames);
            Assert.DoesNotContain(nameof(EntityWithStatics.Counter), type.MemberNames);
            Assert.Contains(nameof(EntityWithStatics.Name), type.MemberNames);
        }

        [Fact]
        public void map_type_with_static_properties()
        {
            EntityWithStatics.Counter = 5;

            var dto = new DtoWithStatics { Name = "name", Inner = new InnerWithStatics { Value = 3 } };

            EntityWithStatics entity = mapper.Map<DtoWithStatics, EntityWithStatics>(dto);

            Assert.Equal("name", entity.Name);
            Assert.Equal(3, entity.Inner.Value);
            Assert.Equal(5, EntityWithStatics.Counter);
        }

        public class DtoWithStatics
        {
            public static int Counter { get; set; } = 42;

            public string Name { get; set; }

            public InnerWithStatics Inner { get; set; }
        }

        public class EntityWithStatics
        {
            public static EntityWithStatics Default { get; } = new EntityWithStatics();

            public static int Counter { get; set; }

            public string Name { get; set; }

            public InnerWithStatics Inner { get; set; }
        }

        public class InnerWithStatics
        {
            public static InnerWithStatics Empty => new InnerWithStatics();

            public int Value { get; set; }
        }
    }
}
