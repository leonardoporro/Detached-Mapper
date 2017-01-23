using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Detached.Mvc.Localization
{
    public interface IJsonStringLocalizerFactory : IStringLocalizerFactory
    {
        IReadOnlyList<CultureInfo> Cultures { get; }

        CultureInfo DefaultCulture { get; }

        IReadOnlyList<string> Modules { get; }

        Regex Pattern { get; set; }

        void Configure(string sourcePath, CultureInfo defaultCulture);
    }
}