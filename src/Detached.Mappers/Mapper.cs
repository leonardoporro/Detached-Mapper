using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypeOptions.Class;
using Detached.PatchTypes;
using System;

namespace Detached.Mappers
{
    public class Mapper : IPatchTypeInfoProvider
    { 
        readonly MapperOptions _options; 
 
        public Mapper(MapperOptions options = null)
        {
            _options = options ?? new MapperOptions();
        }

        bool IPatchTypeInfoProvider.ShouldPatch(Type type)
        {
            return !typeof(IPatch).IsAssignableFrom(type) && _options.GetTypeOptions(type).IsComplex();
        }
 
        public virtual object Map(object source, Type sourceType, object target, Type targetType, IMapContext context = default)
        {
            if (context == null)
            {
                context = new MapContext();
            }

            return _options
                .GetTypeMapper(new TypePair(sourceType, targetType, TypePairFlags.Root | TypePairFlags.Owned))
                .Map(source, target, context);
        }

        public virtual TTarget Map<TSource, TTarget>(TSource source, TTarget target = default, IMapContext context = default)
        {
            if (context == null)
            {
                context = new MapContext();
            }

            return (TTarget)_options
                .GetTypeMapper(new TypePair(typeof(TSource), typeof(TTarget), TypePairFlags.Root | TypePairFlags.Owned))
                .Map(source, target, context);
        }
    }
}