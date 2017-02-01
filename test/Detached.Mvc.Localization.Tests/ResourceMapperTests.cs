using Detached.Mvc.Localization.Mapping;
using Detached.Mvc.Tests.ModuleMock.FeatureMock;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Xunit;

namespace Detached.Mvc.Tests
{
    public class ResourceMapperTests
    {
        [Fact]
        public void when_type_is_provided__resource_key_is_generated__expressions()
        {
            // GIVEN: a resource mapper
            ResourceMapperOptions options = new ResourceMapperOptions();
            options.Rules.AddRange(new[]
            {
                new Rule(
                    expression: new Regex(@"\bSystem.ComponentModel.DataAnnotations.\b(?<class>[\w]+)Attribute\#(?<property>[\w]+)$"),
                    keyTemplate: "Validation_{class}_{property}",
                    sourceTemplate: "Core"
                ),
                new Rule(
                    expression: new Regex(@"(?<module>[\w]+)\.(?<feature>[\w]+)\.(?<model>[\w]+)\#(?<field>[\w]+)\!(?<descriptor>[\w]+)$"),
                    keyTemplate: "{feature}_{model}_{field}_{descriptor}",
                    sourceTemplate: "{module}"
                ),
                new Rule(
                    expression: new Regex(@"(?<module>[\w]+)\.(?<feature>[\w]+)\.(?<model>[\w]+)\!(?<descriptor>[\w]+)$"),
                    keyTemplate: "{feature}_{model}_{descriptor}",
                    sourceTemplate: "{module}"
                )
            });

            ResourceMapper resourceMapper = new ResourceMapper(options);

            // WHEN: a raw full qualified name is provided
            ResourceKey rawKey = resourceMapper.GetKey("Module.Feature.Model#Field!Descriptor");

            // THEN: a valid resource key is returned.
            Assert.Equal("Feature_Model_Field_Descriptor", rawKey.Name);
            Assert.Equal("Module", rawKey.Source);
            Assert.Null(rawKey.Location);

            // WHEN: a model type, field and descriptor is provided
            ResourceKey propKey = resourceMapper.GetFieldKey<ClassMock>(m => m.PropertyMock, "DisplayName");

            // THEN: a valid resource key is returned.
            Assert.Equal("FeatureMock_ClassMock_PropertyMock_DisplayName", propKey.Name);
            Assert.Equal("ModuleMock", propKey.Source);
            Assert.Null(propKey.Location);

            // WHEN: a model type is provided
            ResourceKey modelKey = resourceMapper.GetModelKey<ClassMock>("DisplayName");

            // THEN: a valid resource key is returned
            Assert.Equal("FeatureMock_ClassMock_DisplayName", modelKey.Name);
            Assert.Equal("ModuleMock", modelKey.Source);
            Assert.Null(modelKey.Location);

            // WHEN: a specially mapped class is provided
            ResourceKey validatorKey = resourceMapper.GetFieldKey<RequiredAttribute>(r => r.ErrorMessage);

            // THEN: a valid key is provided
            Assert.Equal("Validation_Required_ErrorMessage", validatorKey.Name);
            Assert.Equal("Core", validatorKey.Source);
        }

        [Fact]
        public void when_type_is_provided__resource_key_is_generated__patterns()
        {
            // GIVEN: a resource mapper
            ResourceMapperOptions options = new ResourceMapperOptions();
            options.Rules.AddRange(new[]
            {
                new Rule(
                    pattern: "System.ComponentModel.DataAnnotations.{class}Attribute#{property}",
                    keyTemplate: "Validation_{class}_{property}",
                    sourceTemplate: "Core"
                ),
                new Rule(
                    pattern: "{module}.{feature}.{model}#{field}!{descriptor}",
                    keyTemplate: "{feature}_{model}_{field}_{descriptor}",
                    sourceTemplate: "{module}"
                ),
                new Rule(
                    pattern: "{module}.{feature}.{model}!{descriptor}",
                    keyTemplate: "{feature}_{model}_{descriptor}",
                    sourceTemplate:"{module}"
                )
            });

            ResourceMapper resourceMapper = new ResourceMapper(options);

            // WHEN: a raw full qualified name is provided
            ResourceKey rawKey = resourceMapper.GetKey("Module.Feature.Model#Field!Descriptor");

            // THEN: a valid resource key is returned.
            Assert.Equal("Feature_Model_Field_Descriptor", rawKey.Name);
            Assert.Equal("Module", rawKey.Source);
            Assert.Null(rawKey.Location);

            // WHEN: a model type, field and descriptor is provided
            ResourceKey propKey = resourceMapper.GetFieldKey<ClassMock>(m => m.PropertyMock, "DisplayName");

            // THEN: a valid resource key is returned.
            Assert.Equal("FeatureMock_ClassMock_PropertyMock_DisplayName", propKey.Name);
            Assert.Equal("ModuleMock", propKey.Source);
            Assert.Null(propKey.Location);

            // WHEN: a model type is provided
            ResourceKey modelKey = resourceMapper.GetModelKey<ClassMock>("DisplayName");

            // THEN: a valid resource key is returned
            Assert.Equal("FeatureMock_ClassMock_DisplayName", modelKey.Name);
            Assert.Equal("ModuleMock", modelKey.Source);
            Assert.Null(modelKey.Location);

            // WHEN: a specially mapped class is provided
            ResourceKey validatorKey = resourceMapper.GetFieldKey<RequiredAttribute>(r => r.ErrorMessage);

            // THEN: a valid key is provided
            Assert.Equal("Validation_Required_ErrorMessage", validatorKey.Name);
            Assert.Equal("Core", validatorKey.Source);
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