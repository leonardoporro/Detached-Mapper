using Detached.Mappers.TypePairs;

namespace Detached.Mappers.TypeMappers
{
    public class LazyTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
    {
        readonly Mapper _mapper;
        readonly TypePair _typePair;

        public LazyTypeMapper(Mapper mapper, TypePair typePair)
        {
            _mapper = mapper;
            _typePair = typePair;
        }

        private ITypeMapper<TSource, TTarget> _value = null;

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (_value == null)
            {
                _value = (ITypeMapper<TSource, TTarget>)_mapper.GetTypeMapper(_typePair);
            }

            return _value.Map(source, target, context);
        }
    }
}