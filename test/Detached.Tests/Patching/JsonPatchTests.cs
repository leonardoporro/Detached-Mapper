using Detached.Model;
using Detached.Patching;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Detached.Tests.Patching
{
    public class JsonPatchTests
    {
        [Fact]
        public void deserialize_as_patch()
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new PatchJsonConverterFactory(Options.Create(new MapperModelOptions())));
            jsonOptions.IgnoreReadOnlyProperties = true;

            string json = @"
                {
                    'Id': 1,
                    'Name': 'test name'
                }".Replace("'", "\"");
            
            Entity entity = JsonSerializer.Deserialize<Entity>(json, jsonOptions);

            IPatch changeTracking = (IPatch)entity;

            Assert.True(changeTracking.IsSet("Name"));
            Assert.True(changeTracking.IsSet("Id"));
            Assert.False(changeTracking.IsSet("Date"));
        }

        [Fact]
        public void deserialize_list_as_patch()
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new PatchJsonConverterFactory(Options.Create(new MapperModelOptions())));

            string json = @"
                [
                    {
                        'Id': 1,
                        'Name': 'test name'
                    },
                    {
                        'Id': 2,
                        'Name': 'test name 2'
                    }

                ]".Replace("'", "\"");

            List<Entity> entities = JsonSerializer.Deserialize<List<Entity>>(json, jsonOptions);

            Assert.True(((IPatch)entities[0]).IsSet("Name"));
            Assert.True(((IPatch)entities[0]).IsSet("Id"));
            Assert.False(((IPatch)entities[0]).IsSet("Date"));

            Assert.True(((IPatch)entities[1]).IsSet("Name"));
            Assert.True(((IPatch)entities[1]).IsSet("Id"));
            Assert.False(((IPatch)entities[1]).IsSet("Date"));
        }

        public class Entity
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime Date { get; set; }
        }
    }
}