using System;
using System.Reflection;
using Detached.Mappers.TypeMappers;

namespace Detached.Mappers.HotChocolate.TypeMappers.POCO.Optional 
{
    public class OptionalTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget> 
    {
        private readonly PropertyInfo _isEmptyProperty;
        private readonly PropertyInfo _valueProperty;

        public OptionalTypeMapper() 
        {
            var type = typeof(TSource);
            _isEmptyProperty = type.GetProperty("IsEmpty");
            _valueProperty = type.GetProperty("Value");
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context) 
        {
            if (source is null || _isEmptyProperty.GetValue(source) is true)
                return target;

            var value = _valueProperty.GetValue(source);

            return (TTarget)Convert.ChangeType(value, typeof(TTarget));
        }
    }
}
