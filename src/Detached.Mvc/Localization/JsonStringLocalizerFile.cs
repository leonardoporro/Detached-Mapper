using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization
{
    public class JsonStringLocalizerFile
    {
        IFileSystem _fileSystem;
        bool _loaded = false;

        public JsonStringLocalizerFile(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string Path { get; set; }

        public string Module { get; set; }

        public CultureInfo Culture { get; set; }

        public void EnsureLoaded()
        {
            if (!_loaded)
            {
                Load();
                _loaded = true;
            }
        }

        public Dictionary<string, LocalizedString> Strings { get; } = new Dictionary<string, LocalizedString>();

        public void Load()
        {
            JObject root;
            using (Stream fileStream = _fileSystem.OpenRead(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                root = (JObject)serializer.Deserialize(new JsonTextReader(new StreamReader(fileStream)));
            }
            Strings.Clear();
            AddValues(root, null);
        }

        void AddValues(JObject jObject, string basePath)
        {
            foreach (var property in jObject)
            {
                var currentPath = basePath == null ? property.Key : basePath + "." + property.Key;
                var value = property.Value;
                if (value.Type == JTokenType.String)
                {
                    Strings[currentPath] = new LocalizedString(currentPath, (string)value);
                }
                else
                {
                    AddValues((JObject)value, currentPath);
                }
            }
        }

        public override string ToString()
        {
            return $"Module: {Module}, Culture: {Culture}, Path: {Path}";
        }
    }
}