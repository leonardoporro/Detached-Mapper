using System.Text.RegularExpressions;

namespace Detached.Mvc.Localization.Mapping
{
    public class Rule
    {
        #region Fields

        static Regex _templateExpression = new Regex(@"\{(?<token>[\w]+)\}", RegexOptions.IgnoreCase);
        Regex _expression;
        string _mapName;
        string _keyTemplate;
        string _sourceTemplate;
        string _locationTemplate;

        #endregion

        #region Ctor.

        public Rule(Regex expression, string keyTemplate, string sourceTemplate, string locationTemplate = null)
        {
            _expression = expression;
            _keyTemplate = keyTemplate;
            _sourceTemplate = sourceTemplate;
            _locationTemplate = locationTemplate;
        }

        public Rule(string pattern, string keyTemplate, string sourceTemplate, string locationTemplate = null)
        {
            string regex = Regex.Escape(pattern);
            regex = regex.Replace(@"\{", "{");
            regex = _templateExpression.Replace(regex, m =>
            {
                string token = m.Groups["token"].Value;
                return $@"(?<{token}>[\w]+)";
            });
            _expression = new Regex("^" + regex + "$", RegexOptions.IgnoreCase);
            _keyTemplate = keyTemplate;
            _sourceTemplate = sourceTemplate;
            _locationTemplate = locationTemplate;
        }

        #endregion

        #region Properties

        public string MapName
        {
            get
            {
                return _mapName;
            }
        }

        public string KeyNameTemplate
        {
            get
            {
                return _keyTemplate;
            }
        }

        public string SourceTemplate
        {
            get
            {
                return _sourceTemplate;
            }
        }

        public string LocationTemplate
        {
            get
            {
                return _locationTemplate;
            }
        }

        #endregion

        public ResourceKey TryGetKey(string fullQualifiedName, ResourceMapperOptions options)
        {
            ResourceKey key = null;

            Match match = _expression.Match(fullQualifiedName);
            if (match.Success)
            {
                key = new ResourceKey();
                key.Name = Resolve(KeyNameTemplate, match, options);
                key.Source = Resolve(SourceTemplate, match, options);
                if (LocationTemplate != null)
                    key.Location = Resolve(LocationTemplate, match, options);
            }

            return key;
        }

        protected string Resolve(string template, Match fullQualifiedMatch, ResourceMapperOptions options)
        {
            return _templateExpression.Replace(template, m =>
            {
                string token = m.Groups["token"].Value;
                string tokenValue = fullQualifiedMatch.Groups[token].Value;

                switch (options.StringCase)
                {
                    case StringCase.CamelCase:
                        tokenValue = tokenValue.Substring(0, 1).ToLower() + tokenValue.Substring(1);
                        break;
                    case StringCase.PascalCase:
                        tokenValue = tokenValue.Substring(0, 1).ToUpper() + tokenValue.Substring(1);
                        break;
                    case StringCase.LowerCase:
                        tokenValue = tokenValue.ToLower();
                        break;
                    case StringCase.UpperCase:
                        tokenValue = tokenValue.ToUpper();
                        break;
                }

                return tokenValue;
            });
        }
    }
}