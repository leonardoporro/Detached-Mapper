using Detached.Annotations;
using Detached.Mappers.Annotations;
using Detached.Mappers.Annotations.Handlers;
using Detached.Mappers.TypeBinders;
using Detached.Mappers.TypeBinders.Binders;
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
using Detached.PatchTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Detached.Mappers
{
    public class MapperOptions : IPatchTypeInfoProvider
    {
        readonly ConcurrentDictionary<Type, IType> _configuredTypes = new();
        readonly ConcurrentDictionary<Type, IType> _allTypes = new();
        readonly ConcurrentDictionary<TypeMapperKey, TypePair> _typePairs = new();

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
                { typeof(MapIgnoreAttribute), new MapIgnoreAnnotationHandler() },
                { typeof(ParentAttribute), new ParentAnnotationHandler() },
                { typeof(AbstractAttribute), new AbstractAnnotationHandler() },
                { typeof(PrimitiveAttribute), new PrimitiveAnnotationHandler() }
            };

            TypeBinders = new List<ITypeBinder>
            {
                new PrimitiveTypeBinder(),
                new ComplexTypeBinder(),
                new CollectionTypeBinder(),
                new InheritedTypeBinder(),
                new NullableTypeBinder()
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

        public virtual List<ITypeBinder> TypeBinders { get; }

        public virtual ClassTypeBuilder<TType> Type<TType>()
        {
            IType type = GetTypeConfiguration(typeof(TType));

            return new ClassTypeBuilder<TType>((ClassType)type, this);
        }

        public virtual IType GetType(Type clrType)
        {
            return _allTypes.GetOrAdd(clrType, clrType =>
            {
                IType type = GetTypeConfiguration(clrType);

                ApplyAnnotations(type);

                return type;
            });
        }

        IType GetTypeConfiguration(Type clrType)
        {
            return _configuredTypes.GetOrAdd(clrType, clrType =>
            {
                IType type = null;

                for (int i = TypeFactories.Count - 1; i >= 0; i--)
                {
                    type = TypeFactories[i].Create(this, clrType);
                    if (type != null)
                        break;
                }

                if (type == null)
                {
                    throw new InvalidOperationException($"Can't get options for type {clrType.GetFriendlyName()}.");
                }

                return type;
            });
        }

        void ApplyAnnotations(IType type)
        {
            foreach (Attribute annotation in type.ClrType.GetCustomAttributes())
            {
                if (AnnotationHandlers.TryGetValue(annotation.GetType(), out IAnnotationHandler handler))
                {
                    handler.Apply(annotation, this, type, null);
                }
            }

            if (type.MemberNames != null)
            {
                foreach (var memberName in type.MemberNames)
                {
                    var member = type.GetMember(memberName);

                    var propInfo = member.GetPropertyInfo();

                    if (propInfo != null)
                    {
                        foreach (Attribute annotation in propInfo.GetCustomAttributes())
                        {
                            if (AnnotationHandlers.TryGetValue(annotation.GetType(), out IAnnotationHandler handler))
                            {
                                handler.Apply(annotation, this, type, member);
                            }
                        }
                    }
                }
            }

            for (int i = TypeConventions.Count - 1; i >= 0; i--)
            {
                TypeConventions[i].Apply(this, type);
            }
        }

        public TypePair GetTypePair(IType sourceType, IType targetType, TypePairMember parentMember)
        {
            return _typePairs.GetOrAdd(new TypeMapperKey(sourceType, targetType, parentMember), key =>
            {
                return TypePairFactory.Create(this, sourceType, targetType, parentMember);
            });
        }

        public virtual bool IsPrimitive(Type type)
        {
            return Primitives.Contains(type)
                || type.IsEnum
                || type.IsGenericType && Primitives.Contains(type.GetGenericTypeDefinition());
        }

        public virtual bool ShouldMap(IType sourceType, IType targetType)
        {
            return sourceType != targetType
                    || sourceType.Annotations.Abstract().Value()
                    || targetType.Annotations.Abstract().Value()
                    || (targetType.IsComplex() || targetType.IsCollection() && GetType(targetType.ItemClrType).IsComplex());
        }

        public virtual bool ShouldPatch(Type type)
        {
            return !typeof(IPatch).IsAssignableFrom(type) && GetType(type).IsComplex();
        }
    }
}