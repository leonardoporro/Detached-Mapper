using Detached.Annotations;
using Detached.Mappers.Types;
using Xunit;

namespace Detached.Mappers.Annotation.Tests
{
    public class AbstractAnnotationTest
    {
        [Fact]
        public void attribute_must_set_annotation()
        {
            Mapper mapper = new Mapper();
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));

            Assert.True(type.Annotations.TryGetValue(AbstractAnnotationHandlerExtensions.VALUE_KEY, out var value) && Equals(value, true));
        }

        [Fact]
        public void fluent_must_set_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<Entity>().Abstract();

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));

            Assert.True(type.Annotations.TryGetValue(AbstractAnnotationHandlerExtensions.VALUE_KEY, out var value) && Equals(value, true));
        }

        [Fact]
        public void fluent_must_unset_annotation()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<AnnotatedEntity>().Abstract(false);

            Mapper mapper = new Mapper(mapperOptions);
            IType type = mapper.Options.GetType(typeof(AnnotatedEntity));

            type.Annotations.TryGetValue(AbstractAnnotationHandlerExtensions.VALUE_KEY, out var value);
            Assert.Equal(false, value);
        }

        [Abstract]
        public class AnnotatedEntity
        {

        }

        public class Entity
        {

        }
    }
}