using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Detached.Mvc.Localization.Mapping
{
    public class ResourceMapper : IResourceMapper
    {
        #region Fields

        ConcurrentDictionary<string, ResourceKey> _cache = new ConcurrentDictionary<string, ResourceKey>();
        ResourceMapperOptions _options = new ResourceMapperOptions();

        #endregion

        #region Ctor.

        public ResourceMapper(ResourceMapperOptions options)
        {
            _options = options;
        }

        #endregion

        public ResourceKey GetKey(string fullQualifiedName)
        {
            return _cache.GetOrAdd(fullQualifiedName, fqn =>
            {
                ResourceKey key = null;
                foreach (MapRule map in _options.Rules)
                {
                    key = map.TryGetKey(fqn, _options);
                    if (key != null)
                        break;
                }

                return key;
            });
        }

        public ResourceKey GetFieldKey<TModel>(Expression<Func<TModel, object>> fieldSelector, string descriptor = null)
        {
            string fieldName = ((MemberExpression)fieldSelector.Body).Member.Name;
            return GetFieldKey(typeof(TModel), fieldName, descriptor);
        }

        public ResourceKey GetFieldKey(Type modelType, string fieldName, string descriptor = null)
        {
            StringBuilder keyBuilder = new StringBuilder();
            keyBuilder.Append(modelType.FullName);
            if (fieldName != null)
            {
                keyBuilder.Append("#");
                keyBuilder.Append(fieldName);
            }
            if (descriptor != null)
            {
                keyBuilder.Append("!");
                keyBuilder.Append(descriptor);
            }
            return GetKey(keyBuilder.ToString());
        }

        public ResourceKey GetModelKey(Type modelType, string descriptor = null)
        {
            StringBuilder keyBuilder = new StringBuilder();
            keyBuilder.Append(modelType.FullName);
            if (descriptor != null)
            {
                keyBuilder.Append("!");
                keyBuilder.Append(descriptor);
            }
            return GetKey(keyBuilder.ToString());
        }

        public ResourceKey GetModelKey<TModel>(string descriptor = null)
        {
            return GetModelKey(typeof(TModel), descriptor);
        }
    }
}