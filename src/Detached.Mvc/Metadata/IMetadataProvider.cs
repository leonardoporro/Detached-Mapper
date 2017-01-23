using System;
using System.Collections.Generic;

namespace Detached.Mvc.Metadata
{
    public interface IMetadataProvider
    {
        List<Pattern> Patterns { get; }

        string Resolve(string template, Type type, string propertyName = null, string metaPropertyName = null);

        TypeMetadata GetTypeMetadata(Type type);

        PropertyMetadata GetPropertyMetadata(Type containerType, string propertyName, string metaPropertyName);
    }
}