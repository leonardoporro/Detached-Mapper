using Detached.Annotations;
using Detached.Model;
using Detached.Patch;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using Xunit;

namespace Detached.Tests.Patch
{
    public class JsonPatchTests
    {
        [Fact]
        public void deserialize_as_patch()
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new PatchJsonConverterFactory(Options.Create(new ModelOptions())));
            jsonOptions.IgnoreReadOnlyProperties = true;

            string json = @"
                {
                    'Id': 1,
                    'Name': 'test name'
                }";
            
            Entity entity = JsonSerializer.Deserialize<Entity>(json.Replace("'", "\""), jsonOptions);

            IPatch changeTracking = (IPatch)entity;

            Assert.True(changeTracking.IsSet("Name"));
            Assert.True(changeTracking.IsSet("Id"));
            Assert.False(changeTracking.IsSet("Date"));
        }

        [Patch]
        public class Entity
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime Date { get; set; }
        }
    }
}