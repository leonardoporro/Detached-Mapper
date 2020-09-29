using AgileObjects.ReadableExpressions.Extensions;
using Detached.Mappers.Reflection;
using System;

namespace Detached.Mappers.Model.Types.Dictionary
{
    public class DictionaryOptionsFactory : ITypeOptionsFactory
    {
        readonly DictionaryTypeOptions _typeOptions = new DictionaryTypeOptions();

        public ITypeOptions Create(MapperModelOptions options, Type type)
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