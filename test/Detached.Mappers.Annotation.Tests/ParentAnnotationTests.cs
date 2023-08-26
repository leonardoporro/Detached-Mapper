using Detached.Annotations;
using Detached.Mappers.Types;
using Xunit;

namespace Detached.Mappers.Annotation.Tests
{
    public class ParentAnnotationTests
    {
        [Fact]
        public void attribute_must_set_annotation()
        {
            Mapper mapper = new Mapper();
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));
            ITypeMember member = type.GetMember("Items");

            Assert.True(member.Annotations.ContainsKey(ParentAnnotationHandlerExtensions.KEY));
        }

        [Fact]
        public void fluent_must_set_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<Entity>()
                .Member(e => e.Items)
                .Parent();

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));
            ITypeMember member = type.GetMember("Items");

            Assert.True(member.Annotations.ContainsKey(ParentAnnotationHandlerExtensions.KEY));
        }

        [Fact]
        public void fluent_must_unset_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<AnnotatedEntity>().Abstract(false);

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));

            Assert.False(type.Annotations.ContainsKey(AbstractAnnotationHandlerExtensions.KEY));
        }

        public class AnnotatedEntity
        {
            [Parent]
            public List<AnnotatedEntity>? Items { get; set; }
        }

        public class Entity
        {
            public List<Entity>? Items { get; set; }
        }
    }
}
