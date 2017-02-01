using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Detached.Mvc.Localization.Json
{
    public class JsonStringLocalizerFile
    {
        #region Fields

        IFileSystem _fileSystem;
        string _fullPath;

        #endregion

        #region Ctor.

        public JsonStringLocalizerFile(IFileSystem fileSystem, string fullPath)
        {
            _fileSystem = fileSystem;
            _fullPath = fullPath;
        }

        #endregion

        #region Properties

        public string Path { get; private set; }

        public bool IsLoaded { get; private set; }

        public Dictionary<string, LocalizedString> Values { get; } = new Dictionary<string, LocalizedString>();

        public bool Exists { get; private set; }

        #endregion

        public void Load()
        {
            Exists = _fileSystem.Exists(_fullPath);
            if (Exists)
            {
                using (Stream fileStream = _fileSystem.OpenRead(_fullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    JObject jObj = (JObject)serializer.Deserialize(new JsonTextReader(new StreamReader(fileStream)));
                    AddValues(jObj, null);
                }
            }
            IsLoaded = true;
        }

        public void EnsureLoaded()
        {
            if (!IsLoaded)
                Load();
        }

        private void AddValues(JObject jObj, string basePath)
        {
            foreach (var property in jObj)
            {
                var currentPath = basePath == null ? property.Key : basePath + "." + property.Key;
                var value = property.Value;
                if (value.Type == JTokenType.String)
                {
                    Values[currentPath] = new LocalizedString(currentPath, (string)value);
                }
                else
                {
                    AddValues((JObject)value, currentPath);
                }
            }
        }

        public LocalizedString GetString(string name)
        {
            LocalizedString value;
            if (!Values.TryGetValue(name, out value))
                value = new LocalizedString(name, name, true);
            return value;
        }
    }
}