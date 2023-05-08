using Detached.Annotations;
using Detached.Mappers.Types;
using Xunit;

namespace Detached.Mappers.Annotation.Tests
{
    public class EntityAnnotationTest
    {
        [Fact]
        public void attribute_must_set_annotation()
        {
            Mapper mapper = new Mapper();
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));

            Assert.True(type.Annotations.ContainsKey(EntityAnnotationHandlerExtensions.KEY));
        }

        [Fact]
        public void fluent_must_set_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<Entity>().Entity();

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));

            Assert.True(type.Annotations.ContainsKey(EntityAnnotationHandlerExtensions.KEY));
        }

        [Fact]
        public void fluent_must_unset_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<AnnotatedEntity>().Entity(false);

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));

            Assert.False(type.Annotations.ContainsKey(EntityAnnotationHandlerExtensions.KEY));
        }

        [Entity]
        public class AnnotatedEntity
        {

        }

        public class Entity
        {

        }
    }
}