using Detached.Mvc.Metadata;
using Detached.Mvc.Tests.ModuleMock.FeatureMock;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Detached.Mvc.Tests
{
    public class MetadataProviderTests
    {
        [Fact]
        public void Test()
        {
            MetadataProvider metadataProvider = new MetadataProvider();
            metadataProvider.Patterns.Clear();
            metadataProvider.Patterns.AddRange(new[]
            {
                new Pattern(@"\bSystem.ComponentModel.DataAnnotations.\b(?<class>[\w]+)Attribute(?:\+(?<property>[\w]+))?$",
                            new Dictionary<string, string> { { "module", "core" }, { "feature", "validation" } }),

                new Pattern(@"(?<module>[\w]+)\.(?<feature>[\w]+)\.(?<class>[\w]+)(?:\+(?<property>[\w]+))?(?:\#(?<metaproperty>[\w]+))?$"),
            });

            string modelKey = metadataProvider.Resolve("{module}.{feature}.{class}", typeof(ClassMock));
            Assert.Equal("moduleMock.featureMock.classMock", modelKey);

            string propKey = metadataProvider.Resolve("{module}.{feature}.{class}.{property}", typeof(ClassMock), nameof(ClassMock.PropertyMock), "DisplayName");
            Assert.Equal("moduleMock.featureMock.classMock.propertyMock", propKey);

            string validatorKey = metadataProvider.Resolve("{module}.{feature}.{class}.{property}", typeof(RequiredAttribute), "ErrorMessage");
            Assert.Equal("core.validation.required.errorMessage", validatorKey);
        }
    }
}

namespace Detached.Mvc.Tests.ModuleMock.FeatureMock
{
    public class ClassMock
    {
        public string PropertyMock { get; set; }
    }
}