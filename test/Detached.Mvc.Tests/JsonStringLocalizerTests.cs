using Detached.Mvc.Localization.Json;
using Microsoft.Extensions.Localization;
using Moq;
using System.Globalization;
using System.IO;
using System.Text;
using Xunit;

namespace Detached.Mvc.Tests
{
    public class JsonStringLocalizerTests
    {
        IFileSystem GetFileSystemMock()
        {
            Mock<IFileSystem> fsMock = new Mock<IFileSystem>();
            fsMock.Setup(f => f.Exists(It.IsAny<string>()))
                  .Returns<string>(f => f ==  @".\lang\res.en.json" || f == @".\lang\res.en-US.json");

            fsMock.Setup(f => f.OpenRead(It.IsAny<string>()))
                  .Returns<string>(path =>
                  {
                      string json = "";
                      switch (path)
                      {
                          case @".\lang\res.en-US.json":
                              json = "{ 'users': { 'values': { 'greeting': 'hello!' } } }".Replace("'", "\"");
                              break;
                          case @".\lang\res.en.json":
                              json = "{ 'users': { 'values': { 'greeting': 'hallo!' } } }".Replace("'", "\"");
                              break;
                      }
                      return new MemoryStream(Encoding.UTF8.GetBytes(json));
                  });

            return fsMock.Object;
        }

        [Fact]
        public void when_existing_key_and_culture_requested__string_is_retrieved()
        {
            // GIVEN: a string localizer factory
            IStringLocalizerFactory factory = new JsonStringLocalizerFactory(
                GetFileSystemMock(), 
                new JsonLocalizationOptions { ResourcesPath = @".\lang" });

            IStringLocalizer stringLocalizer = factory.Create("res", null);

            // WHEN: a localization value is requested for a certain culture
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            string localized = stringLocalizer.GetString("users.values.greeting");

            // THEN: a valid value is requested.
            Assert.Equal(localized, "hello!");

            CultureInfo.CurrentUICulture = new CultureInfo("en");
            localized = stringLocalizer.GetString("users.values.greeting");

            Assert.Equal(localized, "hallo!");
        }

        [Fact]
        public void when_non_existing_culture_requested__parent_culture_is_retrieved()
        {
            // GIVEN: a string localizer factory
            IStringLocalizerFactory factory = new JsonStringLocalizerFactory(
                GetFileSystemMock(),
                new JsonLocalizationOptions { ResourcesPath = @".\lang" });

            IStringLocalizer stringLocalizer = factory.Create("res", null);

            // WHEN: a non existent culture is requested
            CultureInfo.CurrentUICulture = new CultureInfo("en-NZ");
            string localizedString = stringLocalizer.GetString("users.values.greeting");

            // THEN: a localized string with NotFound flag is requested
            Assert.Equal(localizedString, "hallo!");
        }

        [Fact]
        public void when_non_existing_culture_requested__empty_string_is_retrieved()
        {
            // GIVEN: a string localizer factory
            IStringLocalizerFactory factory = new JsonStringLocalizerFactory(
                GetFileSystemMock(),
                new JsonLocalizationOptions { ResourcesPath = @".\lang" });

            IStringLocalizer stringLocalizer = factory.Create("res", null);

            // WHEN: a non existent culture is requested
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            LocalizedString localizedString = stringLocalizer.GetString("users.values.greeting");

            // THEN: a localized string with NotFound flag is requested
            Assert.True(localizedString.ResourceNotFound);
        }

        [Fact]
        public void when_non_existing_key_requested__empty_string_is_retrieved()
        {
            // GIVEN: a string localizer factory
            IStringLocalizerFactory factory = new JsonStringLocalizerFactory(
                GetFileSystemMock(),
                new JsonLocalizationOptions { ResourcesPath = @".\lang" });

            IStringLocalizer stringLocalizer = factory.Create("res", null);

            // WHEN: a non existent culture is requested
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            LocalizedString localizedString = stringLocalizer.GetString("users.values.non-existing");

            // THEN: a localized string with NotFound flag is requested
            Assert.True(localizedString.ResourceNotFound);
        }
    }
}
