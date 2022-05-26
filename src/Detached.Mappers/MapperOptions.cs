using AgileObjects.ReadableExpressions.Extensions;
using Detached.Annotations;
using Detached.Mappers.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.MapperFactories;
using Detached.Mappers.MapperFactories.Entity;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;
using Detached.Mappers.TypeOptions.Class.Conventions;
using Detached.Mappers.TypeOptions.Dictionary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers
{
    public class MapperOptions
    {
        readonly ConcurrentDictionary<Type, ITypeOptions> _options = new ConcurrentDictionary<Type, ITypeOptions>();
        readonly ConcurrentDictionary<TypePair, ITypeMapper> _mappers = new ConcurrentDictionary<TypePair, ITypeMapper>();

        public MapperOptions()
        {
            Factories = new List<TypeMappers.ITypeMapperFactory>
            {
                new TypeMappers.POCO.Collection.CollectionTypeMapperFactory(this),
                new TypeMappers.POCO.Complex.ComplexTypeMapperFactory(this),
                new TypeMappers.POCO.Primitive.PrimitiveTypeMapperFactory(),
                new TypeMappers.POCO.Abstract.AbstractTypeMapperFactory(this),
                new TypeMappers.POCO.Nullable.NullableTypeMapperFactory(this),
                new TypeMappers.POCO.Inherited.InheritedTypeMapperFactory(this)
            };
        }

        public virtual HashSet<Type> Primitives { get; } = new HashSet<Type>
        {
            typeof(bool),
            typeof(string),
            typeof(char),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            typeof(byte),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        public virtual List<ITypeOptionsFactory> TypeOptionsFactories { get; set; }
            = new List<ITypeOptionsFactory>
            {
                new ClassTypeOptionsFactory(),
                new DictionaryTypeOptionsFactory()
            };

        public virtual List<MapperFactory> MapperFactories { get; set; } = new List<MapperFactory>
        {
            new ValueMapperFactory(),
            new ComplexTypeMapperFactory(),
            new ListMapperFactory(),
            new EntityRootMapperFactory(),
            new EntityListMapperFactory(),
            new EntityComposedMapperFactory(),
            new EntityAggregatedMapperFactory(),
            new NullableTypeMapperFactory(),
            new ObjectMapperFactory(),
            new ObjectToObjectMapperFactory()
        };

        public virtual List<ITypeOptionsConvention> Conventions { get; set; }
            = new List<ITypeOptionsConvention>
            {
                new KeyOptionsConvention()
            };

        public virtual Dictionary<Type, Type> ConcreteTypes { get; set; } = new Dictionary<Type, Type>
        {
            { typeof(IList<>), typeof(List<>) },
            { typeof(IEnumerable<>), typeof(List<>) },
            { typeof(ICollection<>), typeof(List<>) },
            { typeof(IDictionary<,>), typeof(Dictionary<,>) }
        };

        public Dictionary<Type, IAnnotationHandler> AnnotationHandlers { get; } = new Dictionary<Type, IAnnotationHandler>
        {
            { typeof(KeyAttribute), new KeyAnnotationHandler() },
            { typeof(AggregationAttribute), new AssociationAnnotationHandler() },
            { typeof(CompositionAttribute), new CompositionAnnotationHandler() },
            { typeof(EntityAttribute), new EntityAnnotationHandler() },
            { typeof(NotMappedAttribute), new NotMappedAnnotationHandler() }
        };

        public virtual ClassTypeOptionsBuilder<TType> Configure<TType>()
        {
            return new ClassTypeOptionsBuilder<TType>((ClassTypeOptions)GetTypeOptions(typeof(TType)));
        }

        public virtual ITypeOptions GetTypeOptions(Type type)
        {
            return _options.GetOrAdd(type, keyType =>
            {
                for (int i = TypeOptionsFactories.Count - 1; i >= 0; i--)
                {
                    ITypeOptions typeOptions = TypeOptionsFactories[i].Create(this, keyType);
                    if (typeOptions != null)
                        return typeOptions;
                }

                throw new InvalidOperationException($"Can't get options for type {type.GetFriendlyName()}.");
            });
        }

        public List<TypeMappers.ITypeMapperFactory> Factories { get; }

        public ITypeMapper GetTypeMapper(TypePair typePair)
        {
            return _mappers.GetOrAdd(typePair, t =>
            {
                ITypeOptions sourceType = GetTypeOptions(typePair.SourceType);
                ITypeOptions targetType = GetTypeOptions(typePair.TargetType);

                for (int i = Factories.Count - 1; i >= 0; i--)
                {
                    ITypeMapperFactory factory = Factories[i];

                    if (factory.CanCreate(typePair, sourceType, targetType))
                    {
                        return factory.Create(typePair, sourceType, targetType);
                    }
                }

                throw new MapperException($"No factory for {typePair.SourceType.Name} -> {typePair.TargetType.Name}");
            });
        }

        public ILazyTypeMapper GetLazyTypeMapper(TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return (ILazyTypeMapper)Activator.CreateInstance(lazyType, new object[] { this, typePair });
        }

        public virtual bool ShouldMap(ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType != targetType
                    || sourceType.IsAbstract
                    || targetType.IsAbstract
                    || !targetType.IsPrimitive;
        }
    }
}