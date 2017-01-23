using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization
{
    public class JsonStringLocalizerFile
    {
        IFileSystem _fileSystem;
        bool _loaded = false;
        Regex paramsPattern = new Regex(@"\{\{[\w]+\}\}", RegexOptions.IgnoreCase);

        public JsonStringLocalizerFile(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string Path { get; set; }

        public string Module { get; set; }

        public CultureInfo Culture { get; set; }

        public Dictionary<string, LocalizedString> Strings { get; } = new Dictionary<string, LocalizedString>();

        public void EnsureLoaded()
        {
            if (!_loaded)
            {
                Load();
                _loaded = true;
            }
        }

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
                    string text = (string)value;
                    text = ReplaceNamedWithPositionalValues(text);
                    Strings[currentPath] = new LocalizedString(currentPath, text);
                }
                else
                {
                    AddValues((JObject)value, currentPath);
                }
            }
        }

        string ReplaceNamedWithPositionalValues(string value)
        {
            int i = 0;
            return paramsPattern.Replace(value, m =>
            {
                string result = "{" + i + "}";
                i++;
                return result;
            });
        }

        public override string ToString()
        {
            return $"Module: {Module}, Culture: {Culture}, Path: {Path}";
        }
    }
}