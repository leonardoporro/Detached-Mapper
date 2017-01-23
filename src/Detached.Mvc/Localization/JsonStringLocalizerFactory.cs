using Detached.Mvc.Metadata;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Localization;
using System.Reflection;

namespace Detached.Mvc.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory, IJsonStringLocalizerFactory
    {
        #region Fields

        const string DEFAULT_MODULE = "DEFAULT_MODULE";
        IFileSystem _fileSystem;
        IMetadataProvider _metadataProvider;
        ConcurrentDictionary<string, JsonStringLocalizerModule> _modules = new ConcurrentDictionary<string, JsonStringLocalizerModule>();
        HashSet<CultureInfo> _cultures = new HashSet<CultureInfo>();
        CultureInfo _defaultCulture;

        #endregion

        #region Ctor.

        public JsonStringLocalizerFactory(IFileSystem fileSytem, IMetadataProvider metadataProvider)
        {
            _fileSystem = fileSytem;
            _metadataProvider = metadataProvider;
        }

        #endregion

        #region Properties

        public Regex Pattern { get; set; } = new Regex(@"(?<name>[a-z]+)_(?:(?<module>[a-z]+)_)?(?<culture>[a-z]{2}(?:\-[a-z]{2})?)\b.json\b$", RegexOptions.IgnoreCase);

        public IReadOnlyList<string> Modules
        {
            get
            {
                return _modules.Keys.ToList();
            }
        }

        public IReadOnlyList<CultureInfo> Cultures
        {
            get
            {
                return _cultures.ToList();
            }
        }

        public CultureInfo DefaultCulture
        {
            get
            {
                return _defaultCulture;
            }
        }

        #endregion

        public void Configure(string sourcePath, CultureInfo defaultCulture)
        {
            _defaultCulture = defaultCulture;
            _modules.Clear();
            string[] filePaths = _fileSystem.GetFiles(sourcePath);
            foreach (string filePath in filePaths)
            {
                Match match = Pattern.Match(filePath);
                if (match.Success)
                {
                    string culture = match.Groups["culture"].Value;
                    string module = match.Groups["module"].Value;
                    if (string.IsNullOrEmpty(module))
                        module = DEFAULT_MODULE;

                    JsonStringLocalizerFile jsonFile = new JsonStringLocalizerFile(_fileSystem)
                    {
                        Path = filePath,
                        Module = module,
                        Culture = new CultureInfo(culture),
                    };
                    _cultures.Add(jsonFile.Culture);
                    _modules.GetOrAdd(module, key => new JsonStringLocalizerModule(defaultCulture))
                            .Files[jsonFile.Culture] = jsonFile;
                }
            }

            if (_modules.Count == 0)
                throw new Exception($"No localization files has been found at '{sourcePath}'. Please check if the directory contains files that can be matched by Pattern property.");
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            JsonStringLocalizerModule module = null;
            TypeMetadata typeMetadata = _metadataProvider.GetTypeMetadata(resourceSource);

            if (typeMetadata == null || !_modules.TryGetValue(typeMetadata.Module, out module))
            {
                if (!_modules.TryGetValue(DEFAULT_MODULE, out module))
                {
                }
            }

            return module;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return null;
        }
    }
}
