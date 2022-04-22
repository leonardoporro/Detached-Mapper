using AgileObjects.ReadableExpressions.Extensions;
using Detached.Annotations;
using Detached.Mappers.Annotations;
using Detached.Mappers.Factories;
using Detached.Mappers.Factories.Entity;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Types.Class;
using Detached.Mappers.TypeOptions.Types.Class.Builder;
using Detached.Mappers.TypeOptions.Types.Class.Conventions;
using Detached.Mappers.TypeOptions.Types.Dictionary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers
{
    public class MapperOptions
    {
        readonly ConcurrentDictionary<Type, ITypeOptions> _options;

        public MapperOptions()
        {
            _options = new ConcurrentDictionary<Type, ITypeOptions>();
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
            typeof(Guid),
            typeof(Nullable<>)
        };

        public virtual List<ITypeOptionsFactory> TypeOptionsFactories { get; set; }
            = new List<ITypeOptionsFactory>
            {
                new ClassOptionsFactory(),
                new DictionaryOptionsFactory()
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
            { typeof(ICollection<>), typeof(List<>) }
        };

        public Dictionary<Type, IAnnotationHandler> AnnotationHandlers { get; } = new Dictionary<Type, IAnnotationHandler>
        {
            { typeof(KeyAttribute), new KeyAnnotationHandler() },
            { typeof(AggregationAttribute), new AssociationAnnotationHandler() },
            { typeof(CompositionAttribute), new CompositionAnnotationHandler() },
            { typeof(EntityAttribute), new EntityAnnotationHandler() },
            { typeof(NotMappedAttribute), new NotMappedAnnotationHandler() }
        };

        public virtual IEnumerable<ITypeOptions> TypeOptions => _options.Values;

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
    }
}