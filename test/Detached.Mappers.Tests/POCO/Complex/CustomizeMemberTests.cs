using Detached.PatchTypes;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Complex
{
    public class CustomizeMemberTests
    {
        [Fact]
        public void customize_property()
        {
            MapperOptions mapperOptions = new MapperOptions();
            mapperOptions.Type<TargetEntity>()
                .Member(m => m.Value)
                .Setter((@this, value, mapContext) => { @this.Value = value + 1; });

            mapperOptions.Type<SourceEntity>()
                .Member(m => m.Value)
                .Getter((@this, mapContext) => { return @this.Value + 1; });

            Mapper mapper = new Mapper(mapperOptions);

            var result = mapper.Map<SourceEntity, TargetEntity>(new SourceEntity { Value = 2 });

            bool contains = result.IsSet(nameof(TargetEntity.Value));
            Assert.True(contains);
            Assert.Equal(4, result.Value);
        }

        public class SourceEntity
        {
            public int Value { get; set; }
        }

        public class TargetEntity : IPatch
        {
            int _value;

            public int Value
            {
                get { return _value; }
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        _modified.Add(nameof(Value));
                    }
                }
            }

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