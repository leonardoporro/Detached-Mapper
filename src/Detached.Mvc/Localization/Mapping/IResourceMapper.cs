using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mvc.Localization.Mapping
{
    public interface IResourceMapper
    {
        ResourceKey GetKey(string fullName);

        ResourceKey GetKey(string namespaceOrPrefix, string typeName, string fieldOrAction, string descriptor = "Value");

        ResourceKey GetFallbackKey(string feature, string modelOrType, string descriptor);

        string[] GetSupportInfoArguments();
    }
}