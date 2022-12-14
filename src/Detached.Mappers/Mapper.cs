using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.PatchTypes;
using System;

namespace Detached.Mappers
{
    public class Mapper : IPatchTypeInfoProvider
    { 
        readonly MapperOptions _mapperOptions; 
 
        public Mapper(MapperOptions options = null)
        {
            _mapperOptions = options ?? new MapperOptions();
        }

        bool IPatchTypeInfoProvider.ShouldPatch(Type type)
        {
            return !typeof(IPatch).IsAssignableFrom(type) && _mapperOptions.GetType(type).IsComplex();
        }
 
        public virtual object Map(object source, Type sourceClrType, object target, Type targetClrType, IMapContext context = default)
        {
            if (context == null)
            {
                context = new MapContext();
            }

            IType sourceType = _mapperOptions.GetType(sourceClrType);
            IType targetType = _mapperOptions.GetType(targetClrType);
            TypePair typePair = _mapperOptions.GetTypePair(sourceType, targetType, null);
            ITypeMapper typeMapper = _mapperOptions.GetTypeMapper(typePair);

            return typeMapper.Map(source, target, context);
        }

        public virtual TTarget Map<TSource, TTarget>(TSource source, TTarget target = default, IMapContext context = default)
        {
            return (TTarget)Map(source, typeof(TSource), target, typeof(TTarget), context);
        }
    }
}