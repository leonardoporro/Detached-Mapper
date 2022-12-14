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
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;
using Detached.Mappers.Types.Conventions;
using Detached.Mappers.Types.Dictionary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers
{
    public class MapperOptions
    {
        readonly ConcurrentDictionary<Type, IType> _types = new ConcurrentDictionary<Type, IType>();
        readonly ConcurrentDictionary<(IType, IType), TypePair> _typePairs = new ConcurrentDictionary<(IType, IType), TypePair>();
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

            TypeFactories = new List<ITypeFactory>
            {
                new ClassTypeFactory(),
                new DictionaryTypeFactory()
            };

            TypeConventions = new List<ITypeConvention>
            {
                new KeyConvention()
            };

            TypePairFactory = new TypePairFactory();

            TypeMapperFactories = new List<ITypeMapperFactory>
            {
                new CollectionTypeMapperFactory(),
                new ComplexTypeMapperFactory(),
                new PrimitiveTypeMapperFactory(),
                new AbstractTypeMapperFactory(),
                new NullableTypeMapperFactory(),
                new InheritedTypeMapperFactory(),
                new EntityCollectionTypeMapperFactory(),
                new EntityTypeMapperFactory(),
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

        public virtual List<ITypeFactory> TypeFactories { get; }

        public virtual ITypePairFactory TypePairFactory { get; set; }

        public List<ITypeMapperFactory> TypeMapperFactories { get; }

        public virtual List<ITypeConvention> TypeConventions { get; }

        public virtual List<IPropertyNameConvention> PropertyNameConventions { get; }

        public virtual Dictionary<Type, Type> ConcreteTypes { get; }

        public virtual bool MergeCollections { get; set; } = false;

        public virtual ClassTypeBuilder<TType> Type<TType>()
        {
            return new ClassTypeBuilder<TType>((ClassType)GetType(typeof(TType)), this);
        }

        [Obsolete("Use Type<TType>()")]
        public virtual ClassTypeBuilder<TType> Configure<TType>()
        {
            return new ClassTypeBuilder<TType>((ClassType)GetType(typeof(TType)), this);
        }

        public virtual IType GetType(Type type)
        {
            return _types.GetOrAdd(type, keyType =>
            {
                for (int i = TypeFactories.Count - 1; i >= 0; i--)
                {
                    IType typeOptions = TypeFactories[i].Create(this, keyType);
                    if (typeOptions != null)
                        return typeOptions;
                }

                throw new InvalidOperationException($"Can't get options for type {type.GetFriendlyName()}.");
            });
        }

        public TypePair GetTypePair(IType sourceType, IType targetType, TypePairMember parentMember)
        {
            return _typePairs.GetOrAdd((sourceType, targetType), k =>
            {
                return TypePairFactory.Create(this, sourceType, targetType, parentMember);
            });
        }

        public ITypeMapper GetTypeMapper(TypePair typePair)
        {
            return _typeMappers.GetOrAdd(typePair, t =>
            {
                for (int i = TypeMapperFactories.Count - 1; i >= 0; i--)
                {
                    ITypeMapperFactory factory = TypeMapperFactories[i];

                    if (factory.CanCreate(this, typePair))
                    {
                        return factory.Create(this, typePair);
                    }
                }

                throw new MapperException($"No factory for {typePair.SourceType.ClrType.GetFriendlyName()} -> {typePair.TargetType.ClrType.GetFriendlyName()}");
            });
        }

        public string GetSourcePropertyName(IType sourceType, IType targetType, string memberName)
        {
            for (int i = PropertyNameConventions.Count - 1; i >= 0; i--)
            {
                memberName = PropertyNameConventions[i].GetSourcePropertyName(sourceType, targetType, memberName);
            }

            return memberName;
        }

        public ILazyTypeMapper GetLazyTypeMapper(TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);
            return (ILazyTypeMapper)Activator.CreateInstance(lazyType, new object[] { this, typePair });
        }

        public virtual bool ShouldMap(IType sourceType, IType targetType)
        {
            return sourceType != targetType
                    || sourceType.IsAbstract()
                    || targetType.IsAbstract()
                    || !targetType.IsPrimitive();
        }

        public virtual bool IsPrimitive(Type type)
        {
            if (type.IsEnum)
                return true;
            else if (type.IsGenericType)
                return Primitives.Contains(type.GetGenericTypeDefinition());
            else
                return Primitives.Contains(type);
        }
    }
}