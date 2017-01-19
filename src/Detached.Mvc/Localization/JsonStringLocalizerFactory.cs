using Detached.Mvc.Metadata;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Detached.Mvc.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        #region Fields

        const string DEFAULT_MODULE = "*";
        IFileSystem _fileSystem;
        IMetadataProvider _metadataProvider;
        ConcurrentDictionary<string, JsonStringLocalizerModule> _modules = new ConcurrentDictionary<string, JsonStringLocalizerModule>();

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

        public ICollection<string> Modules { get; } = new HashSet<string>();

        public ICollection<CultureInfo> Cultures { get; } = new HashSet<CultureInfo>();

        #endregion

        public void Configure(string sourcePath, CultureInfo defaultCulture)
        {
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
                    else
                        Modules.Add(module);

                    JsonStringLocalizerFile jsonFile = new JsonStringLocalizerFile(_fileSystem)
                    {
                        Path = filePath,
                        Module = module,
                        Culture = new CultureInfo(culture),
                    };
                    Cultures.Add(jsonFile.Culture);
                    _modules.GetOrAdd(module, key => new JsonStringLocalizerModule(defaultCulture))
                            .Files[jsonFile.Culture] = jsonFile;
                }
            }
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            TypeMetadata typeMetadata = _metadataProvider.GetTypeMetadata(resourceSource);
            JsonStringLocalizerModule module;
            if (!_modules.TryGetValue(typeMetadata.Module, out module))
            {
                if (!_modules.TryGetValue(DEFAULT_MODULE, out module))
                {
                    throw new Exception("No localization modules has been loaded.");
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
