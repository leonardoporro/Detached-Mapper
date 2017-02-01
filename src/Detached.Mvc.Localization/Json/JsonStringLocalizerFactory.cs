using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace Detached.Mvc.Localization.Json
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        #region Fields

        ConcurrentDictionary<string, JsonStringLocalizer> _localizers = new ConcurrentDictionary<string, JsonStringLocalizer>();
        IFileSystem _fileSystem;
        JsonLocalizationOptions _options;

        #endregion

        #region Ctor.

        public JsonStringLocalizerFactory(IFileSystem fileSystem, JsonLocalizationOptions options)
        {
            _fileSystem = fileSystem;
            _options = options;
        }

        #endregion

        public IStringLocalizer Create(Type resourceSource)
        {
            return Create(resourceSource.FullName, null);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            string fullPath;
            if (location != null)
                fullPath = Path.Combine(_options.ResourcesPath, location, baseName);
            else
                fullPath = Path.Combine(_options.ResourcesPath, baseName);

            return _localizers.GetOrAdd(fullPath, fp => new JsonStringLocalizer(fp, _fileSystem));
        }
    }
}