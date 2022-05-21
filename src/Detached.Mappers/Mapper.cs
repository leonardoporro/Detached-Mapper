using AgileObjects.ReadableExpressions;
using Detached.Mappers.Context;
using Detached.Mappers.Exceptions;
using Detached.Mappers.Factories;
using Detached.Mappers.TypeMaps;
using Detached.Mappers.TypeOptions;
using Detached.PatchTypes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers
{
    public class Mapper : IPatchTypeInfoProvider
    {
        readonly TypeMapFactory _typeMapFactory;
        readonly MapperOptions _options;
        readonly IMemoryCache _memoryCache;

        public Mapper()
        {
            _options = new MapperOptions();
            _typeMapFactory = new TypeMapFactory();;
            _memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions { SizeLimit = null }));
        }

        public Mapper(MapperOptions options)
        {
            _options = options;
            _typeMapFactory = new TypeMapFactory();
            _memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions { SizeLimit = null }));
        }

        public Mapper(
            MapperOptions options,
            TypeMapFactory typeMapFactory,
            IMemoryCache memoryCache)
        {
            _options = options;
            _typeMapFactory = typeMapFactory;
            _memoryCache = memoryCache;
        }

        public TTarget Map<TSource, TTarget>(TSource source, TTarget target = default, IMapperContext context = default)
        {
            MapperCacheKey key = new MapperCacheKey(typeof(TSource), typeof(TTarget), true);

            return ((MapperDelegate<TSource, TTarget>)_memoryCache.GetOrCreate(key, entry =>
            {
                entry.SetSize(1);

                TypeMap typeMap = GetTypeMap(typeof(TSource), typeof(TTarget));
                LambdaExpression map = GetFactory(typeMap).Create(typeMap);

                Debug.WriteLine(map.ToReadableString());

                return map.Compile();
            }))(source, target, context);
        }

        public object Map(object source, Type sourceType, object target, Type targetType, IMapperContext context = default)
        {
            MapperCacheKey key = new MapperCacheKey(sourceType, targetType, false);

            return _memoryCache.GetOrCreate(key, entry =>
            {
                entry.SetSize(1);

                ITypeOptions sourceOptions = _options.GetTypeOptions(sourceType);
                ITypeOptions targetOptions = _options.GetTypeOptions(targetType);

                if (ShouldMap(sourceOptions, targetOptions))
                {
                    return Lambda(
                          typeof(MapperDelegate),
                          Parameter("sourceObj", typeof(object), out Expression sourceObjExpr),
                          Parameter("targetObj", typeof(object), out Expression targetObjExpr),
                          Parameter("context", typeof(IMapperContext), out Expression contextExpr),
                          Convert(
                              Call("Map",
                                  Constant(this),
                                  new[] { sourceType, targetType },
                                  Convert(sourceObjExpr, sourceType),
                                  Convert(targetObjExpr, targetType),
                                  contextExpr
                              ),
                              typeof(object)
                          )
                      ).Compile() as MapperDelegate;
                }
                else
                {
                    return Lambda(
                          typeof(MapperDelegate),
                          Parameter("sourceObj", typeof(object), out Expression sourceObjExpr),
                          Parameter("targetObj", typeof(object), out Expression targetObjExpr),
                          Parameter("context", typeof(IMapperContext), out Expression contextExpr),
                          Result(sourceObjExpr)
                      ).Compile() as MapperDelegate;
                }
               
            })(source, target, context);
        }

        public virtual bool ShouldMap(ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType != targetType || !targetType.IsValue;
        }

        public MapperFactory GetFactory(TypeMap typeMap)
        {
            for (int i = _options.MapperFactories.Count - 1; i >= 0; i--)
            {
                if (_options.MapperFactories[i].CanMap(typeMap))
                {
                    return _options.MapperFactories[i];
                }
            }

            throw new MapperException($"Can't map {typeMap.SourceTypeOptions.Type} to {typeMap.TargetTypeOptions.Type}");
        }

        public ITypeOptions GetTypeOptions(Type type) => _options.GetTypeOptions(type);

        public TypeMap GetTypeMap(Type sourceType, Type targetType)
            => _typeMapFactory.Create(this, _options, null, sourceType, targetType, true);

        bool IPatchTypeInfoProvider.ShouldPatch(Type type)
        {
            return !typeof(IPatch).IsAssignableFrom(type) && GetTypeOptions(type).IsComplexType;
        }
    }
}