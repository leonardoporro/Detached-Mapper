using Detached.Mappers.TypePairs;

namespace Detached.Mappers.TypeMappers
{
    public class LazyTypeMapper<TSource, TTarget> : ILazyTypeMapper
    {
        readonly Mapper _mapper;
        readonly TypePair _typePair;

        public LazyTypeMapper(Mapper mapper, TypePair typePair)
        {
            _mapper = mapper;
            _typePair = typePair;
        }

        private ITypeMapper<TSource, TTarget> _value = null;

        public ITypeMapper<TSource, TTarget> Value
        {
            get
            {
                if (_value == null)
                {
                    _value = (ITypeMapper<TSource, TTarget>)_mapper.GetTypeMapper(_typePair);
                }

                return _value;
            }
        }

        ITypeMapper ILazyTypeMapper.Value => Value;
    }
}