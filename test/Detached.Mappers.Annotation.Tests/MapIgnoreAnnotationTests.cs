using Detached.Annotations;
using Detached.Mappers.Types;
using Xunit;

namespace Detached.Mappers.Annotation.Tests
{
    public class MapIgnoreAnnotationTests
    {
        [Fact]
        public void attribute_must_set_annotation()
        {
            Mapper mapper = new Mapper();
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));
            ITypeMember member = type.GetMember("Items");

            Assert.True(member.Annotations.TryGetValue(MapIgnoreAnnotationHandlerExtensions.VALUE_KEY, out var value) && Equals(value, true));
        }

        [Fact]
        public void fluent_must_set_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<Entity>()
                .Member(e => e.Items)
                .Exclude();

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));
            ITypeMember member = type.GetMember("Items");

            Assert.True(member.Annotations.TryGetValue(MapIgnoreAnnotationHandlerExtensions.VALUE_KEY, out var value) && Equals(value, true));
        }

        [Fact]
        public void fluent_must_unset_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<AnnotatedEntity>().Member(e => e.Items).Include();

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));
            ITypeMember member = type.GetMember("Items");

            member.Annotations.TryGetValue(MapIgnoreAnnotationHandlerExtensions.VALUE_KEY, out var value);
            Assert.Equal(false, value);
        }

        public class AnnotatedEntity
        {
            [MapIgnore]
            public List<AnnotatedEntity>? Items { get; set; }
        }

        public class Entity
        {
            public List<Entity>? Items { get; set; }
        }
    }
}
