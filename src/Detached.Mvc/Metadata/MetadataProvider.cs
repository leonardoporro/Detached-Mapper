using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Detached.Mvc.Metadata
{
    public enum StringCase { LowerCase = 0, UpperCase = 1, PascalCase = 2, CamelCase = 3 }

    public class MetadataProvider : IMetadataProvider
    {
        #region Fields

        Func<string, string>[] caseStrategy = new Func<string, string>[] {
            s => s.ToLower(),
            s => s.ToUpper(),
            s => s.Substring(0, 1).ToUpper() + s.Substring(1),
            s => s.Substring(0, 1).ToLower() + s.Substring(1)
        };
        ConcurrentDictionary<string, Dictionary<string, string>> metadataCache = new ConcurrentDictionary<string, Dictionary<string, string>>();

        #endregion

        #region Ctor.

        public MetadataProvider()
        {
        }

        #endregion

        #region Properties

        public List<Pattern> Patterns { get; } = new List<Pattern>();

        public Regex TemplatePattern { get; set; } = new Regex(@"\{(?<token>[\w]+)\}");

        public StringCase StringCase { get; set; } = StringCase.CamelCase;

        #endregion


        Dictionary<string, string> GetMetadataValues(Type containerType, string propertyName, string metaPropertyName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(containerType.FullName);
            if (propertyName != null)
            {
                builder.Append("+");
                builder.Append(propertyName);
            }
            if (metaPropertyName != null)
            {
                builder.Append("#");
                builder.Append(metaPropertyName);
            }

            return metadataCache.GetOrAdd(builder.ToString(), key =>
            {
                Dictionary<string, string> metadata = null;
                foreach (Pattern regex in Patterns)
                {
                    if (regex.TryGetMetadata(key,  out metadata))
                        break;
                }

                if (metadata == null)
                {
                    throw new Exception($"Can't provide metadata for type {containerType.FullName}.");
                }

                return metadata;
            });
        }

        public TypeMetadata GetTypeMetadata(Type type)
        {
            return new TypeMetadata(GetMetadataValues(type, null, null));
        }

        public PropertyMetadata GetPropertyMetadata(Type containerType, string propertyName, string metaPropertyName)
        {


            PropertyMetadata propMetadata = new PropertyMetadata(GetMetadataValues(containerType, propertyName, metaPropertyName));
            if (propMetadata.Property == null)
                propMetadata.Property = propertyName;
            if (propMetadata.MetaProperty == null)
                propMetadata.MetaProperty = metaPropertyName;

            return propMetadata;
        }

        public string Resolve(string template, Type type, string propertyName = null, string metaPropertyName = null)
        {
            Dictionary<string, string> metadata = GetMetadataValues(type, propertyName, metaPropertyName);
            if (metadata != null)
            {
                return TemplatePattern.Replace(template, m =>
                {
                    string token = m.Groups["token"].Value;
                    string tokenValue;
                    metadata.TryGetValue(token, out tokenValue);

                    tokenValue = caseStrategy[(int)StringCase](tokenValue);
                    return tokenValue;
                });
            }
            else
            {
                return null;
            }
        }
    }
}