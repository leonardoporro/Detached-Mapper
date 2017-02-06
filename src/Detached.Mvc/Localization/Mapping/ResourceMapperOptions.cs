using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization.Mapping
{
    public enum StringCase { LowerCase, UpperCase, PascalCase, CamelCase }

    public delegate ResourceKey FallbackResourceKeyDelegate(string feature, string modelOrType, string property);

    public class ResourceMapperOptions
    {
        public List<Rule> Rules { get; set; } = new List<Rule>();

        public StringCase StringCase { get; set; } = StringCase.CamelCase;

        public FallbackResourceKeyDelegate FallbackKey { get; set; }

        public ISupportInfo SupportInfo { get; set; } = new SupportInfo();
    }
}
