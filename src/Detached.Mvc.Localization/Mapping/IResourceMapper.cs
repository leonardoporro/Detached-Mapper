using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mvc.Localization.Mapping
{
    public interface IResourceMapper
    {
        ResourceKey GetFieldKey(Type modelType, string fieldName, string descriptor = null);

        ResourceKey GetFieldKey<TModel>(Expression<Func<TModel, object>> fieldSelector, string descriptor = null);

        ResourceKey GetKey(string fullQualifiedName);

        ResourceKey GetModelKey(Type modelType, string descriptor = null);

        ResourceKey GetModelKey<TModel>(string descriptor = null);
    }
}