using AgileObjects.ReadableExpressions.Extensions;
using Detached.Annotations;
using Detached.Mappers.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypeMappers.Entity.Collection;
using Detached.Mappers.TypeMappers.Entity.Complex;
using Detached.Mappers.TypeMappers.POCO.Abstract;
using Detached.Mappers.TypeMappers.POCO.Collection;
using Detached.Mappers.TypeMappers.POCO.Complex;
using Detached.Mappers.TypeMappers.POCO.Inherited;
using Detached.Mappers.TypeMappers.POCO.Nullable;
using Detached.Mappers.TypeMappers.POCO.Primitive;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;
using Detached.Mappers.TypeOptions.Conventions;
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
        readonly ConcurrentDictionary<Type, ITypeOptions> _typeOptions = new ConcurrentDictionary<Type, ITypeOptions>();
        readonly ConcurrentDictionary<TypePair, ITypeMapper> _typeMappers = new ConcurrentDictionary<TypePair, ITypeMapper>();

        public MapperOptions()
        {
            Primitives = new HashSet<Type>
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

            TypeOptionsFactories = new List<ITypeOptionsFactory>
            {
                new ClassTypeOptionsFactory(),
                new DictionaryTypeOptionsFactory()
            };

            TypeMapperFactories = new List<ITypeMapperFactory>
            {
                new CollectionTypeMapperFactory(this),
                new ComplexTypeMapperFactory(this),
                new PrimitiveTypeMapperFactory(),
                new AbstractTypeMapperFactory(this),
                new NullableTypeMapperFactory(this),
                new InheritedTypeMapperFactory(this),
                new EntityCollectionTypeMapperFactory(this),
                new EntityTypeMapperFactory(this),
            };

            TypeConventions = new List<ITypeOptionsConvention>
            {
                new KeyOptionsConvention()
            };

            ConcreteTypes = new Dictionary<Type, Type>
            {
                { typeof(IList<>), typeof(List<>) },
                { typeof(IEnumerable<>), typeof(List<>) },
                { typeof(ICollection<>), typeof(List<>) },
                { typeof(IDictionary<,>), typeof(Dictionary<,>) }
            };

            AnnotationHandlers = new Dictionary<Type, IAnnotationHandler>
            {
                { typeof(KeyAttribute), new KeyAnnotationHandler() },
                { typeof(AggregationAttribute), new AggregationAnnotationHandler() },
                { typeof(CompositionAttribute), new CompositionAnnotationHandler() },
                { typeof(EntityAttribute), new EntityAnnotationHandler() },
                { typeof(NotMappedAttribute), new NotMappedAnnotationHandler() },
                { typeof(ParentAttribute), new ParentAnnotationHandler() }
            };

            PropertyNameConventions = new List<IPropertyNameConvention>();
        }

        public virtual HashSet<Type> Primitives { get; }

        public Dictionary<Type, IAnnotationHandler> AnnotationHandlers { get; }

        public virtual List<ITypeOptionsFactory> TypeOptionsFactories { get; }

        public List<ITypeMapperFactory> TypeMapperFactories { get; }

        public virtual List<ITypeOptionsConvention> TypeConventions { get; }

        public virtual List<IPropertyNameConvention> PropertyNameConventions { get; }

        public virtual Dictionary<Type, Type> ConcreteTypes { get; }

        public virtual bool MergeCollections { get; set; } = false;

        public virtual ClassTypeOptionsBuilder<TType> Type<TType>()
        {
            return new ClassTypeOptionsBuilder<TType>((ClassTypeOptions)GetTypeOptions(typeof(TType)));
        }

        [Obsolete("Use Type<TType>()")]
        public virtual ClassTypeOptionsBuilder<TType> Configure<TType>()
        {
            return new ClassTypeOptionsBuilder<TType>((ClassTypeOptions)GetTypeOptions(typeof(TType)));
        }

        public virtual ITypeOptions GetTypeOptions(Type type)
        {
            return _typeOptions.GetOrAdd(type, keyType =>
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

        public ITypeMapper GetTypeMapper(TypePair typePair)
        {
            return _typeMappers.GetOrAdd(typePair, t =>
            {
                ITypeOptions sourceType = GetTypeOptions(typePair.SourceType);
                ITypeOptions targetType = GetTypeOptions(typePair.TargetType);

                for (int i = TypeMapperFactories.Count - 1; i >= 0; i--)
                {
                    ITypeMapperFactory factory = TypeMapperFactories[i];

                    if (factory.CanCreate(typePair, sourceType, targetType))
                    {
                        return factory.Create(typePair, sourceType, targetType);
                    }
                }

                throw new MapperException($"No factory for {typePair.SourceType.GetFriendlyName()} -> {typePair.TargetType.GetFriendlyName()}");
            });
        }

        public string GetSourcePropertyName(ITypeOptions sourceType, ITypeOptions targetType, string memberName)
        {
            for (int i = PropertyNameConventions.Count - 1; i >= 0; i--)
            {
                memberName = PropertyNameConventions[i].GetSourcePropertyName(sourceType, targetType, memberName);
            }

            return memberName;
        }

        public ILazyTypeMapper GetLazyTypeMapper(TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return (ILazyTypeMapper)Activator.CreateInstance(lazyType, new object[] { this, typePair });
        }

        public virtual bool ShouldMap(ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType != targetType
                    || sourceType.IsAbstract()
                    || targetType.IsAbstract()
                    || !targetType.IsPrimitive();
        }
    }
}