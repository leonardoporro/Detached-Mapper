using AgileObjects.ReadableExpressions.Extensions;
using Detached.Mappers.TypeOptions;
using Detached.RuntimeTypes.Reflection;
using System;

namespace Detached.Mappers.TypeOptions.Types.Dictionary
{
    public class DictionaryOptionsFactory : ITypeOptionsFactory
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