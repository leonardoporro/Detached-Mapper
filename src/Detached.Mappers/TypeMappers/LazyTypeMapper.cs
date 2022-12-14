using Detached.Mappers.TypePairs;

namespace Detached.Mappers.TypeMappers
{
    public class LazyTypeMapper<TSource, TTarget> : ILazyTypeMapper
    {
        readonly MapperOptions _options;
        readonly TypePair _typePair;

        public LazyTypeMapper(MapperOptions options, TypePair typePair)
        {
            _options = options;
            _typePair = typePair;
        }

        private ITypeMapper<TSource, TTarget> _value = null;

        public ITypeMapper<TSource, TTarget> Value
        {
            get
            {
                if (_value == null)
                {
                    _value = (ITypeMapper<TSource, TTarget>)_options.GetTypeMapper(_typePair);
                }

                return _value;
            }
        }

        ITypeMapper ILazyTypeMapper.Value => Value;
    }
}