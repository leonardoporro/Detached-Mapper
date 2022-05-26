using AgileObjects.ReadableExpressions.Extensions;
using Detached.RuntimeTypes.Reflection;
using System;

namespace Detached.Mappers.TypeOptions.Dictionary
{
    public class DictionaryTypeOptionsFactory : ITypeOptionsFactory
    {
        readonly DictionaryTypeOptions _typeOptions = new DictionaryTypeOptions();

        public ITypeOptions Create(MapperOptions options, Type type)
        {
            ITypeOptions result = null;

            if (type.IsDictionary(out Type keyType, out Type valueType)
                && keyType == typeof(string)
                && valueType == typeof(object))
            {
                result = _typeOptions;
            }

            return result;
        }
    }
}