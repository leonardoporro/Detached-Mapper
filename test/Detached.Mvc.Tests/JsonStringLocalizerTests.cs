using Detached.Mvc.Localization;
using Detached.Mvc.Metadata;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using Xunit;

namespace Detached.Mvc.Tests.JsonStringLocalizer
{
    public class JsonStringLocalizerTests
    {
        [Fact]
        public void when_configured__schema_is_created()
        {
            // GIVEN: a string localizer factory
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(f => f.GetFiles(It.IsAny<string>(), "*", true))
                          .Returns(GetSetupLocalizationFiles);

            Mock<IMetadataProvider> metadataProviderMock = new Mock<IMetadataProvider>();
            metadataProviderMock.Setup(m => m.GetTypeMetadata(It.IsAny<Type>()))
                                .Returns<Type>(GetModule);

            JsonStringLocalizerFactory factory = new JsonStringLocalizerFactory(fileSystemMock.Object, metadataProviderMock.Object);

            // WHEN: configured
            factory.Configure("mock", new CultureInfo("en-US"));

            // THEN: schema is created
            Assert.Equal(2, factory.Modules.Count);
            Assert.Contains("users", factory.Modules);
            Assert.Contains("core", factory.Modules);

            Assert.Equal(2, factory.Modules.Count);
            Assert.Contains(new CultureInfo("en"), factory.Cultures);
            Assert.Contains(new CultureInfo("en-US"), factory.Cultures);
            Assert.Contains(new CultureInfo("es"), factory.Cultures);
            Assert.Contains(new CultureInfo("es-AR"), factory.Cultures);
        }

        [Fact]
        public void when_existing_key_provided__value_returned()
        {
            // GIVEN: a configured string localizer factory
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(f => f.GetFiles(It.IsAny<string>(), "*", true)).Returns(GetSetupLocalizationFiles);
            fileSystemMock.Setup(f => f.OpenRead(It.IsAny<string>())).Returns<string>(OpenLocalizationFile);

            Mock<IMetadataProvider> metadataProviderMock = new Mock<IMetadataProvider>();
            metadataProviderMock.Setup(m => m.GetTypeMetadata(It.IsAny<Type>())).Returns<Type>(GetModule);

            JsonStringLocalizerFactory factory = new JsonStringLocalizerFactory(fileSystemMock.Object, metadataProviderMock.Object);
            factory.Configure("mock", new CultureInfo("en-US"));

            // WHEN: a string its get by its name
            IStringLocalizer localizer = factory.Create(typeof(UserMockType));

            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            string str = localizer.GetString("users.values.greeting");

            CultureInfo.CurrentUICulture = new CultureInfo("en");
            string str2 = localizer.GetString("users.values.greeting");

            Assert.Equal("hello!", str);
            Assert.Equal("hallo!", str2);
        }

        public void when_non_existing_key_provided__empty_value_returned()
        {
            // GIVEN: a configured string localizer factory
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(f => f.GetFiles(It.IsAny<string>(), "*", true)).Returns(GetSetupLocalizationFiles);
            fileSystemMock.Setup(f => f.OpenRead(It.IsAny<string>())).Returns<string>(OpenLocalizationFile);

            Mock<IMetadataProvider> metadataProviderMock = new Mock<IMetadataProvider>();
            metadataProviderMock.Setup(m => m.GetTypeMetadata(It.IsAny<Type>())).Returns<Type>(GetModule);

            JsonStringLocalizerFactory factory = new JsonStringLocalizerFactory(fileSystemMock.Object, metadataProviderMock.Object);
            factory.Configure("mock", new CultureInfo("en-US"));
            IStringLocalizer localizer = factory.Create(typeof(UserMockType));

            // WHEN: a string its get by its name
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            LocalizedString str = localizer.GetString("users.values.non-existing");

            // THEN: empty value is returned
            Assert.True(str.ResourceNotFound);
        }

        [Fact]
        public void when_non_existing_culture__fallback_to_parent()
        {
            // GIVEN: a configured string localizer factory
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(f => f.GetFiles(It.IsAny<string>(), "*", true)).Returns(GetSetupLocalizationFiles);
            fileSystemMock.Setup(f => f.OpenRead(It.IsAny<string>())).Returns<string>(OpenLocalizationFile);

            Mock<IMetadataProvider> metadataProviderMock = new Mock<IMetadataProvider>();
            metadataProviderMock.Setup(m => m.GetTypeMetadata(It.IsAny<Type>())).Returns<Type>(GetModule);

            JsonStringLocalizerFactory factory = new JsonStringLocalizerFactory(fileSystemMock.Object, metadataProviderMock.Object);
            factory.Configure("mock", new CultureInfo("en-US"));

            IStringLocalizer localizer = factory.Create(typeof(UserMockType));
            
            // WHEN: non-neutral culture does not exist
            CultureInfo.CurrentUICulture = new CultureInfo("en-ZW");
            string str = localizer.GetString("users.values.greeting");

            // THEN: fall back to neutral culture.
            Assert.Equal("hallo!", str);

            // WHEN: culture does not exist at all
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            string str2 = localizer.GetString("users.values.greeting");

            // THEN: fall back to default culture
            Assert.Equal("hello!", str2);
        }

        string[] GetSetupLocalizationFiles()
        {
            return new[]
                {
                    "./lang/res_users_en-US.json",
                    "./lang/res_users_en.json",
                    "./lang/res_core_en-US.json",
                    "./lang/res_core_en.json",
                    "./lang/res_en-US.json",
                    "./lang/res_en.json",
                    "./lang/res_users_es-AR.json",
                    "./lang/res_users_es.json",
                    "./lang/res_core_es-AR.json",
                    "./lang/res_core_es.json",
                    "./lang/res_es-AR.json",
                    "./lang/res_es.json"
                };
        }

        Stream OpenLocalizationFile(string path)
        {
            string json = "";
            switch (path)
            {
                case "./lang/res_users_en-US.json":
                    json = "{ 'users': { 'values': { 'greeting': 'hello!' } } }".Replace("'", "\"");
                    break;
                case "./lang/res_users_en.json":
                    json = "{ 'users': { 'values': { 'greeting': 'hallo!' } } }".Replace("'", "\"");
                    break;
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        } 

        TypeMetadata GetModule(Type type)
        {
            if (type == typeof(UserMockType))
                return new TypeMetadata { Module = "users" };
            else if (type == typeof(CoreMockType))
                return new TypeMetadata { Module = "core" };
            else
                return new TypeMetadata { Module = null };
        }

        class UserMockType
        {
        }

        class CoreMockType
        {
        }
    }
}
