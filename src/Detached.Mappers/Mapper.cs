using Detached.Mappers.Exceptions;
using Detached.Mappers.Extensions;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Detached.Mappers
{
    public class Mapper
    { 
        readonly ConcurrentDictionary<TypePairKey, ITypeMapper> _typeMappers = new ConcurrentDictionary<TypePairKey, ITypeMapper>();

        public Mapper(MapperOptions options = null)
        {
            Options = options ?? new MapperOptions();
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

        public Expression Bind<TSource, TTarget>()
        {
            return null;
        }
    }
}