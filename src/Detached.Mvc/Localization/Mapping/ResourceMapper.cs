using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text;

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

        public virtual ResourceKey GetKey(string fullName)
        {
            return _cache.GetOrAdd(fullName, fqn =>
            {
                ResourceKey key = null;
                foreach (Rule map in _options.Rules)
                {
                    key = map.TryGetKey(fqn, _options);
                    if (key != null)
                        break;
                }

                return key;
            });
        }

        public virtual ResourceKey GetKey(string namespaceOrPrefix, string typeName, string memberName, string descriptor = "Value")
        {
            StringBuilder keyBuilder = new StringBuilder();
            keyBuilder.Append(namespaceOrPrefix);
            keyBuilder.Append(".");
            keyBuilder.Append(typeName);
            if (memberName != null)
            {
                keyBuilder.Append(".");
                keyBuilder.Append(memberName);
            }
            if (descriptor != null)
            {
                keyBuilder.Append("#");
                keyBuilder.Append(descriptor);
            }
            return GetKey(keyBuilder.ToString());
        }

        public virtual ResourceKey GetKey<T>(Expression<Func<T, object>> propertySelector, string descriptor = "Value")
        {
            Type type = typeof(T);
            string propName = ((MemberExpression)propertySelector.Body).Member.Name;
            return GetKey(type.Namespace, type.Name, propName, descriptor);
        }

        public virtual ResourceKey GetFallbackKey(string feature, string modelOrType, string descriptor)
        {
            return _options?.FallbackKey(feature, modelOrType, descriptor);
        }

        public string[] GetSupportInfoArguments()
        {
            return new[] { _options.SupportInfo.Email,
                           _options.SupportInfo.Phone,
                           _options.SupportInfo.Address,
                           _options.SupportInfo.Custom };
        }
    }
}