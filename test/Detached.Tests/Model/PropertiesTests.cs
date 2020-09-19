using Detached.Mapping;
using Detached.Mapping.Context;
using Detached.Model;
using Detached.Patching;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Tests.Model
{
    public class PropertiesTests
    {
        [Fact]
        public void customize_property()
        {
            MapperModelOptions modelOptions = new MapperModelOptions();
            modelOptions.Configure<TargetEntity>()
                        .Member(m => m.Value)
                        .Setter(Lambda(
                                Parameter(typeof(TargetEntity), out Expression entity),
                                Parameter(typeof(int), out Expression value),
                                Parameter(typeof(IMapperContext), out Expression context),
                                Block(
                                    Assign(Property(entity, nameof(TargetEntity.Value)), value),
                                    Call("Add", Field(entity, "_modified"), Constant(nameof(TargetEntity.Value)))
                                )
                            ));

            Mapper mapper = new Mapper(
                Options.Create(modelOptions),
                new TypeMapFactory());

            var result = mapper.Map<SourceEntity, TargetEntity>(new SourceEntity { Value = 2 });

            bool contains = result.IsSet(nameof(TargetEntity.Value));
            Assert.True(contains);
        }

        public class SourceEntity
        {
            public int Value { get; set; }
        }

        public class TargetEntity : IPatch
        {
            public int Value { get; set; }

            readonly HashSet<string> _modified = new HashSet<string>();

            public bool IsSet(string name)
            {
                return _modified.Contains(name);
            }

            public void Reset()
            {
                _modified.Clear();
            }
        }
    }
}