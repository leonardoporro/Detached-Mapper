using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization.Mapping
{
    public enum StringCase { LowerCase, UpperCase, PascalCase, CamelCase }

    public class ResourceMapperOptions
    {
        public List<MapRule> Rules { get; set; } = new List<MapRule>();

        public StringCase StringCase { get; set; } = StringCase.CamelCase;
    }
}
