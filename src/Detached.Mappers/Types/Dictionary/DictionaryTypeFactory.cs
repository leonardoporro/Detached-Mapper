using AgileObjects.ReadableExpressions.Extensions;
using Detached.Mappers.Types;
using Detached.RuntimeTypes.Reflection;
using System;

namespace Detached.Mappers.Types.Dictionary
{
    public class DictionaryTypeFactory : ITypeFactory
    {
        readonly DictionaryType _typeOptions = new DictionaryType();

        public IType Create(MapperOptions options, Type type)
        {
            IType result = null;

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