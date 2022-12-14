using Detached.Mappers.Json.Tests.Fixture;
using Detached.Mappers.Types.Conventions;
using System.Text.Json.Nodes;
using Xunit;

namespace Detached.Mappers.Json.Tests
{
    public class JsonTests
    {
        [Fact]
        public void map_json_to_entity()
        {
            // GIVEN 4 json files using new System.Text.Json.Nodes namespace.
            JsonNode optionsFile = JsonNode.Parse(File.ReadAllText("./Fixture/settings.json"));
            JsonNode optionsFile1 = JsonNode.Parse(File.ReadAllText("./Fixture/settings.1.json"));
            JsonNode optionsFile2 = JsonNode.Parse(File.ReadAllText("./Fixture/settings.2.json"));
            JsonNode optionsFile3 = JsonNode.Parse(File.ReadAllText("./Fixture/settings.3.json"));

            // create the target options class. a single instance.
            Options options = new Options();

            // create options. Json lib is a separate package to avoid adding System.Text.Json dependency to the main lib, call WithJson() to integrate.
            MapperOptions mapperOptions = new MapperOptions().WithJson();

            // this flag indicates the mapper to preserve target items of associated collections. the default option is to remove them.
            // notice that ConnectionOptions is marked as [Entity] and has a [Key]. the library will pair collection items by key 
            // before update, delete or create.
            // an internal hash table is used to speed up this, so the order of the mapped collection is not guaranteed.
            mapperOptions.MergeCollections = true;

            mapperOptions.PropertyNameConventions.Add(new CamelCasePropertyNameConvention());

            // create the mapper.
            Mapper mapper = new Mapper(mapperOptions);

            // WHEN base file is mapped.
            mapper.Map(optionsFile, options);

            // THEN the values are correct.
            Assert.Equal("*", options.AllowedHosts);
            Assert.NotNull(options.Logging);
            Assert.Equal("Debug", options.Logging.Level);
            Assert.NotNull(options.ConnectionStrings);
            Assert.Equal("Server=tcp:myserver1.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn1").Value);
            Assert.Equal("Server=tcp:myserver2.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn2").Value);

            // WHEN first file is mapped.
            mapper.Map(optionsFile1, options);

            // THEN cnn2 connection changes, the rest of the values are preserved.
            Assert.Equal("*", options.AllowedHosts);
            Assert.NotNull(options.Logging);
            Assert.Equal("Debug", options.Logging.Level);
            Assert.NotNull(options.ConnectionStrings);
            Assert.Equal("Server=tcp:myserver1.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn1").Value);
            Assert.Equal("Server=tcp:db.google.com,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn2").Value);

            // WHEN second file is mapped.
            mapper.Map(optionsFile2, options);

            // THEN AllowedHosts changes, the rest of the values are preserved.
            Assert.Equal("subdomain.google.com", options.AllowedHosts);
            Assert.NotNull(options.Logging);
            Assert.Equal("Debug", options.Logging.Level);
            Assert.NotNull(options.ConnectionStrings);
            Assert.Equal("Server=tcp:myserver1.database.windows.net,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn1").Value);
            Assert.Equal("Server=tcp:db.google.com,1433;Database=myDataBase;", options.ConnectionStrings.FirstOrDefault(c => c.Name == "cnn2").Value);

            // WHEN the third file is mapped.
            mapper.Map(optionsFile3, options);

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