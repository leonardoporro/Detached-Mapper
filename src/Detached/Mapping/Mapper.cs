using AgileObjects.ReadableExpressions;
using Detached.Mapping.Context;
using Detached.Mapping.Exceptions;
using Detached.Mapping.Mappers;
using Detached.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping
{
    public class Mapper
    {
        readonly TypeMapFactory _typeMapFactory;

        readonly ConcurrentDictionary<(Type, Type), Delegate> _mappers
            = new ConcurrentDictionary<(Type, Type), Delegate>();

        readonly ConcurrentDictionary<(Type, Type), MapperDelegate> _typedCalls
            = new ConcurrentDictionary<(Type, Type), MapperDelegate>();

        readonly MapperModelOptions _options;

        public Mapper()
        {
            _options = new MapperModelOptions();
            _typeMapFactory = new TypeMapFactory();
        }

        public Mapper(
            IOptions<MapperModelOptions> options,
            TypeMapFactory typeMapFactory)
        {
            _options = options.Value;
            _typeMapFactory = typeMapFactory;
        }

        public TTarget Map<TSource, TTarget>(TSource source, TTarget target = default, IMapperContext context = default)
        {
            var mapper = (MapperDelegate<TSource, TTarget>)_mappers.GetOrAdd((typeof(TSource), typeof(TTarget)), key =>
            {
                TypeMap typeMap = GetTypeMap(typeof(TSource), typeof(TTarget));
                LambdaExpression map = GetFactory(typeMap).Create(typeMap);

                Debug.WriteLine(map.ToReadableString());

                return map.Compile();
            });

            return mapper(source, target, context);
        }

        public object Map(object source, Type sourceType, object target, Type targetType, IMapperContext context = default)
        {
            return _typedCalls.GetOrAdd((sourceType, targetType), k =>
            {
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

            throw new MapperException($"Can't map {typeMap.SourceOptions.Type} to {typeMap.TargetOptions.Type}");
        }

        public ITypeOptions GetTypeOptions(Type type) => _options.GetTypeOptions(type);

        public TypeMap GetTypeMap(Type sourceType, Type targetType) 
            => _typeMapFactory.Create(this, _options, null, sourceType, targetType, true);
    }
}