using Detached.Mappers.Annotations;
using Xunit;

namespace Detached.Mappers.Annotation.Tests
{
    public class SetGenericAnnotation
    {
        [Fact]
        public void Annotation_Set()
        {
            var collection = new AnnotationCollection();

            var annotation = collection.Annotation<bool>("TEST_ANNOTATION");

            Assert.False(annotation.IsDefined());

            annotation.Set(true);

            Assert.True(annotation.IsDefined());
            Assert.True(annotation.Value());

            annotation.Set(false);

            Assert.True(annotation.IsDefined());
            Assert.False(annotation.Value());

            annotation.Reset();

            Assert.False(annotation.IsDefined());
            Assert.False(annotation.Value());
        }
    }
}