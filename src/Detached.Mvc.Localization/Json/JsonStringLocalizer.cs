using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace Detached.Mvc.Localization.Json
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        #region Fields

        string _path;
        IFileSystem _fileSystem;
        ConcurrentDictionary<CultureInfo, JsonStringLocalizerFile> _files = new ConcurrentDictionary<CultureInfo, JsonStringLocalizerFile>();

        #endregion

        #region Ctor.

        public JsonStringLocalizer(string path, IFileSystem fileSystem)
        {
            _path = path;
            _fileSystem = fileSystem;
        }

        #endregion

        JsonStringLocalizerFile GetFile(CultureInfo cultureInfo)
        {
            string fullPath = $"{_path}.{cultureInfo.Name}.json";
            return _files.GetOrAdd(cultureInfo, fp => new JsonStringLocalizerFile(_fileSystem, fullPath));
        }

        public LocalizedString this[string name]
        {
            get
            {
                JsonStringLocalizerFile file = GetFile(CultureInfo.CurrentUICulture);
                file.EnsureLoaded();

                if (!(file.Exists || CultureInfo.CurrentUICulture.IsNeutralCulture))
                {
                    file = GetFile(CultureInfo.CurrentUICulture.Parent);
                    file.EnsureLoaded();
                    if (!file.Exists)
                        return new LocalizedString(name, name, true);
                }
                return file.GetString(name);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                LocalizedString localized = this[name];
                if (!localized.ResourceNotFound)
                    localized = new LocalizedString(name, string.Format(name, arguments));

                return localized;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }
    }
}
