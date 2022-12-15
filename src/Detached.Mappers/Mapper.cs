using Detached.Mappers.Exceptions;
using Detached.Mappers.Extensions;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.PatchTypes;
using System;
using System.Collections.Concurrent;

namespace Detached.Mappers
{
    public class Mapper : IPatchTypeInfoProvider
    { 
        readonly ConcurrentDictionary<TypePairKey, ITypeMapper> _typeMappers = new ConcurrentDictionary<TypePairKey, ITypeMapper>();

        public Mapper(MapperOptions options = null)
        {
            Options = options ?? new MapperOptions();
        }

        bool IPatchTypeInfoProvider.ShouldPatch(Type type)
        {
            return !typeof(IPatch).IsAssignableFrom(type) && Options.GetType(type).IsComplex();
        }

        public MapperOptions Options { get; }

        public void Reset()
        {
            _typeMappers.Clear();
        }
 
        public virtual object Map(object source, Type sourceClrType, object target, Type targetClrType, IMapContext context = default)
        {
            if (context == null)
            {
                context = new MapContext();
            }

            IType sourceType = Options.GetType(sourceClrType);
            IType targetType = Options.GetType(targetClrType);
            TypePair typePair = Options.GetTypePair(sourceType, targetType, null);
            ITypeMapper typeMapper = GetTypeMapper(typePair);

            return typeMapper.Map(source, target, context);
        }

        public virtual TTarget Map<TSource, TTarget>(TSource source, TTarget target = default, IMapContext context = default)
        {
            return (TTarget)Map(source, typeof(TSource), target, typeof(TTarget), context);
        }

        public ITypeMapper GetTypeMapper(TypePair typePair)
        {
            return _typeMappers.GetOrAdd(new TypePairKey(typePair.SourceType, typePair.TargetType, typePair.ParentMember), key =>
            {
                for (int i = Options.TypeMapperFactories.Count - 1; i >= 0; i--)
                {
                    ITypeMapperFactory factory = Options.TypeMapperFactories[i];

                    if (factory.CanCreate(this, typePair))
                    {
                        return factory.Create(this, typePair);
                    }
                }

                throw new MapperException($"No factory for {typePair.SourceType.ClrType.GetFriendlyName()} -> {typePair.TargetType.ClrType.GetFriendlyName()}");
            });
        }

        public ILazyTypeMapper GetLazyTypeMapper(TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);
            return (ILazyTypeMapper)Activator.CreateInstance(lazyType, new object[] { this, typePair });
        }
    }
}