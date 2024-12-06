using Detached.Mappers.Context;
using Detached.Mappers.Options;
using Detached.Mappers.Tests.Json.Fixture;
using Detached.Mappers.TypePairs.Conventions;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using Xunit;

namespace Detached.Mappers.Tests.Json
{
    public class JsonTests
    {
        [Fact]
        public void map_json_to_entity()
        {
            // GIVEN 4 json files using new System.Text.Json.Nodes namespace.
            JsonNode optionsFile = JsonNode.Parse(File.ReadAllText("./Json/Fixture/settings.json"));
            JsonNode optionsFile1 = JsonNode.Parse(File.ReadAllText("./Json/Fixture/settings.1.json"));
            JsonNode optionsFile2 = JsonNode.Parse(File.ReadAllText("./Json/Fixture/settings.2.json"));
            JsonNode optionsFile3 = JsonNode.Parse(File.ReadAllText("./Json/Fixture/settings.3.json"));

            // create the target options class. a single instance.
            AppOptions options = new AppOptions();

            // create options. Json lib is a separate package to avoid adding System.Text.Json dependency to the main lib, call WithJson() to integrate.
            MapperOptions mapperOptions = new MapperOptions();

            mapperOptions.MemberNameConventions.Add(new CamelCaseMemberNameConvention());

            // create the mapper.
            Mapper mapper = new Mapper(mapperOptions);

            // WHEN base file is mapped.
            MapContext mapContext = new MapContext();
            mapContext.Parameters.CompositeCollectionBehavior = CompositeCollectionBehavior.Append;

            mapper.Map(optionsFile, options, mapContext);

            // THEN the values are correct.
            Assert.Equal("*", options.AllowedHosts);
            Assert.NotNull(options.Logging);
            Assert.Equal("Debug", options.Logging.Level);
            Assert.NotNull(options.ConnectionStrings);
            Assert.Equal("Server=tcp:myserver1.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn1").Value);
            Assert.Equal("Server=tcp:myserver2.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn2").Value);

            // WHEN first file is mapped.
            mapper.Map(optionsFile1, options, mapContext);

            // THEN cnn2 connection changes, the rest of the values are preserved.
            Assert.Equal("*", options.AllowedHosts);
            Assert.NotNull(options.Logging);
            Assert.Equal("Debug", options.Logging.Level);
            Assert.NotNull(options.ConnectionStrings);
            Assert.Equal("Server=tcp:myserver1.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn1").Value);
            Assert.Equal("Server=tcp:db.google.com,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn2").Value);

            // WHEN second file is mapped.
            mapper.Map(optionsFile2, options, mapContext);

            // THEN AllowedHosts changes, the rest of the values are preserved.
            Assert.Equal("subdomain.google.com", options.AllowedHosts);
            Assert.NotNull(options.Logging);
            Assert.Equal("Debug", options.Logging.Level);
            Assert.NotNull(options.ConnectionStrings);
            Assert.Equal("Server=tcp:myserver1.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn1").Value);
            Assert.Equal("Server=tcp:db.google.com,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn2").Value);

            // WHEN the third file is mapped.
            mapper.Map(optionsFile3, options, mapContext);

            // THEN only Logging.Level changes.
            Assert.NotNull(options.Logging);
            Assert.Equal("Info", options.Logging.Level);
            Assert.Equal("subdomain.google.com", options.AllowedHosts);
            Assert.NotNull(options.ConnectionStrings);
            Assert.Equal("Server=tcp:myserver1.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn1").Value);
            Assert.Equal("Server=tcp:db.google.com,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn2").Value);
        }
    }
}