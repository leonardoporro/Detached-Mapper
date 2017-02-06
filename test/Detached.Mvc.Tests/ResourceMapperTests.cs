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
            options.StringCase = StringCase.PascalCase;
            options.Rules.AddRange(new[]
            {
                new Rule(
                    expression: new Regex(@"\bSystem.ComponentModel.DataAnnotations.\b(?<class>[\w]+)Attribute\.(?<property>[\w]+)\#(?<descriptor>[\w]+)$"),
                    keyTemplate: "Validation_{class}_{property}",
                    sourceTemplate: "Core"
                ),
                new Rule(
                    expression: new Regex(@"(?<module>[\w]+)\.(?<feature>[\w]+)\.(?<model>[\w]+)\.(?<field>[\w]+)\#(?<descriptor>[\w]+)$"),
                    keyTemplate: "{feature}_{model}_{field}_{descriptor}",
                    sourceTemplate: "{module}"
                )
            });

            ResourceMapper resourceMapper = new ResourceMapper(options);

            // WHEN: a raw full qualified name is provided
            ResourceKey rawKey = resourceMapper.GetKey("Module.Feature.Model.Field#Descriptor");

            // THEN: a valid resource key is returned.
            Assert.Equal("Feature_Model_Field_Descriptor", rawKey.KeyName);
            Assert.Equal("Module", rawKey.ResourceName);
            Assert.Null(rawKey.ResourceLocation);

            // WHEN: a model type, field and descriptor is provided
            ResourceKey propKey = resourceMapper.GetKey<ClassMock>(m => m.PropertyMock, "DisplayName");

            // THEN: a valid resource key is returned.
            Assert.Equal("FeatureMock_ClassMock_PropertyMock_DisplayName", propKey.KeyName);
            Assert.Equal("ModuleMock", propKey.ResourceName);
            Assert.Null(propKey.ResourceLocation);

            // WHEN: a specially mapped class is provided
            ResourceKey validatorKey = resourceMapper.GetKey<RequiredAttribute>(r => r.ErrorMessage);

            // THEN: a valid key is provided
            Assert.Equal("Validation_Required_ErrorMessage", validatorKey.KeyName);
            Assert.Equal("Core", validatorKey.ResourceName);
        }

        [Fact]
        public void when_type_is_provided__resource_key_is_generated__patterns()
        {
            // GIVEN: a resource mapper
            ResourceMapperOptions options = new ResourceMapperOptions();
            options.StringCase = StringCase.PascalCase;
            options.Rules.AddRange(new[]
            {
                new Rule(
                    pattern: "System.ComponentModel.DataAnnotations.{class}Attribute.{property}#{descriptor}",
                    keyTemplate: "Validation_{class}_{property}",
                    sourceTemplate: "Core"
                ),
                new Rule(
                    pattern: "{brand}.{library}.{package}.{module}.{feature}.{model}.{field}#{descriptor}",
                    keyTemplate: "{feature}_{model}_{field}_{descriptor}",
                    sourceTemplate: "{module}"
                )
            });

            ResourceMapper resourceMapper = new ResourceMapper(options);

            // WHEN: a raw full qualified name is provided
            ResourceKey rawKey = resourceMapper.GetKey("Brand.Library.Package.Module.Feature.Model.Field#Descriptor");

            // THEN: a valid resource key is returned.
            Assert.Equal("Feature_Model_Field_Descriptor", rawKey.KeyName);
            Assert.Equal("Module", rawKey.ResourceName);
            Assert.Null(rawKey.ResourceLocation);

            // WHEN: a model type, field and descriptor is provided
            ResourceKey propKey = resourceMapper.GetKey<ClassMock>(m => m.PropertyMock, "DisplayName");

            // THEN: a valid resource key is returned.
            Assert.Equal("FeatureMock_ClassMock_PropertyMock_DisplayName", propKey.KeyName);
            Assert.Equal("ModuleMock", propKey.ResourceName);
            Assert.Null(propKey.ResourceLocation);

            // WHEN: a specially mapped class is provided
            ResourceKey validatorKey = resourceMapper.GetKey<RequiredAttribute>(r => r.ErrorMessage);

            // THEN: a valid key is provided
            Assert.Equal("Validation_Required_ErrorMessage", validatorKey.KeyName);
            Assert.Equal("Core", validatorKey.ResourceName);
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