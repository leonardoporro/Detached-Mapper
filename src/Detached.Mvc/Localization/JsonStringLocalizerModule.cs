using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using JsonFileByCulture = System.Collections.Generic.Dictionary<System.Globalization.CultureInfo, Detached.Mvc.Localization.JsonStringLocalizerFile>;

namespace Detached.Mvc.Localization
{
    public class JsonStringLocalizerModule : IStringLocalizer
    {
        Dictionary<CultureInfo, JsonStringLocalizerFile> _files = new Dictionary<CultureInfo, JsonStringLocalizerFile>();
        CultureInfo _defaultCulture; 

        public JsonStringLocalizerModule(CultureInfo defaultCulture)
        {
            _defaultCulture = defaultCulture;
        }

        public IDictionary<CultureInfo, JsonStringLocalizerFile> Files
        {
            get
            {
                return _files;
            }
        }

        public LocalizedString this[string name]
        {
            get
            {
                LocalizedString value;
                JsonStringLocalizerFile file;
                if (!_files.TryGetValue(CultureInfo.CurrentUICulture, out file))
                {
                    if (!_files.TryGetValue(CultureInfo.CurrentUICulture.Parent, out file))
                    {
                        if (!_files.TryGetValue(_defaultCulture, out file))
                        {
                            return new LocalizedString(name, name, true);
                        }
                    }
                }

                file.EnsureLoaded();
                if (!file.Strings.TryGetValue(name, out value))
                {
                    value = new LocalizedString(name, name, true);
                }

                return value;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                return new LocalizedString(name, string.Format(this[name].Value, arguments));
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _files.Values.SelectMany(f => f.Strings.Values);
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }
    }
}
