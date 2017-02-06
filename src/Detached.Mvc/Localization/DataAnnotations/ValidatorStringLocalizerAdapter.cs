using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization.DataAnnotations
{
    public class ValidatorStringLocalizerAdapter : IStringLocalizer
    {
        IList<IStringLocalizer> _stringLocalizers;
        IList<string> _keys;

        public ValidatorStringLocalizerAdapter(IList<IStringLocalizer> stringLocalizers, IList<string> keys)
        {
            _stringLocalizers = stringLocalizers;
            _keys = keys;
        }

        public LocalizedString this[string name]
        {
            get
            {
                for (int i = 0; i < _stringLocalizers.Count; i++)
                {
                    string key = _keys == null ? name : _keys[i] ?? name;
                    LocalizedString result = _stringLocalizers[i][key];
                    if (!result.ResourceNotFound)
                        return result;
                }

                return new LocalizedString(name, name, true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                for (int i = 0; i < _stringLocalizers.Count; i++)
                {
                    string key = _keys == null ? name : _keys[i] ?? name;
                    LocalizedString result = _stringLocalizers[i][key, arguments];
                    if (!result.ResourceNotFound)
                        return result;
                }

                return new LocalizedString(name, name, true);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _stringLocalizers[0].GetAllStrings(includeParentCultures);
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new ValidatorStringLocalizerAdapter(_stringLocalizers.Select(s => s.WithCulture(culture)).ToArray(), _keys);
        }
    }
}
