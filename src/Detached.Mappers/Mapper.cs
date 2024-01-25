using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeBinders;
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
        readonly ConcurrentDictionary<TypeMapperKey, ITypeMapper> _typeMappers = new ConcurrentDictionary<TypeMapperKey, ITypeMapper>();
        readonly ConcurrentDictionary<TypeBoundKey, Expression> _typeBindings = new ConcurrentDictionary<TypeBoundKey, Expression>();

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

        public Expression<Func<TSource, TTarget>> Bind<TSource, TTarget>()
        {
            return (Expression<Func<TSource, TTarget>>)Bind(typeof(TSource), typeof(TTarget));
        }

        public Expression Bind(Type sourceClrType, Type targetClrType)
        {
            return _typeBindings.GetOrAdd(new TypeBoundKey(sourceClrType, targetClrType), key =>
            {
                var sourceType = Options.GetType(key.SourceClrType);
                var targetType = Options.GetType(key.TargetClrType);
                var typePair = Options.GetTypePair(sourceType, targetType, null);

                var delegateType = typeof(Func<,>).MakeGenericType(sourceClrType, targetClrType);

                var param = Expression.Parameter(sourceClrType, "e");
                var body = GetTypeBinder(typePair).Bind(this, typePair, param);

                return Expression.Lambda(delegateType, body, param);
            });
        }

        public ITypeMapper GetTypeMapper(TypePair typePair)
        {
            return _typeMappers.GetOrAdd(new TypeMapperKey(typePair.SourceType, typePair.TargetType, typePair.ParentMember), key =>
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

        public ITypeBinder GetTypeBinder(TypePair typePair)
        {
            for (int i = Options.TypeBinders.Count - 1; i >= 0; i--)
            {
                ITypeBinder typeBinder = Options.TypeBinders[i];
                if (typeBinder.CanBind(this, typePair))
                {
                    return typeBinder;
                }
            }

            throw new MapperException($"No binder for {typePair.SourceType.ClrType.GetFriendlyName()} -> {typePair.TargetType.ClrType.GetFriendlyName()}");
        }
    }
}