using Detached.Mappers.Exceptions;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using System;
using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.Annotations
{
    public class TypeAnnotationTests
    {
        [Fact]
        public void Annotation_Abstract()
        {
            IType type = new ClassType();

            type.Abstract(true);

            Assert.True(type.Annotations.Abstract().IsDefined());
            Assert.True(type.IsAbstract());

            type.Abstract(false);

            Assert.True(type.Annotations.Abstract().IsDefined());
            Assert.False(type.IsAbstract());

            type.Annotations.Abstract().Reset();

            Assert.False(type.Annotations.Abstract().IsDefined());
            Assert.False(type.IsAbstract());
        }

        [Fact]
        public void Annotation_Entity()
        {
            ClassType type = new ClassType();
            type.MappingSchema = MappingSchema.Complex;

            type.Entity(true);

            Assert.True(type.Annotations.Entity().IsDefined());
            Assert.True(type.IsEntity());

            type.Entity(false);

            Assert.True(type.Annotations.Entity().IsDefined());
            Assert.False(type.IsEntity());

            type.Annotations.Entity().Reset();

            Assert.False(type.Annotations.Entity().IsDefined());
            Assert.False(type.IsEntity());

            type.MappingSchema = MappingSchema.Primitive;

            Assert.Throws<MapperException>(() => type.Entity());
        }

        [Fact]
        public void Annotation_Key()
        {
            ClassTypeMember member = new ClassTypeMember();

            member.Key(true);

            Assert.True(member.Annotations.Key().IsDefined());
            Assert.True(member.IsKey());

            member.Key(false);

            Assert.True(member.Annotations.Key().IsDefined());
            Assert.False(member.IsKey());

            member.Annotations.Key().Reset();

            Assert.False(member.Annotations.Key().IsDefined());
            Assert.False(member.IsKey());
        }

        [Fact]
        public void Annotation_MapIgnore()
        {
            ClassTypeMember member = new ClassTypeMember();

            member.Ignore(true);

            Assert.True(member.Annotations.Ignored().IsDefined());
            Assert.True(member.IsIgnored());

            member.Ignore(false);

            Assert.True(member.Annotations.Ignored().IsDefined());
            Assert.False(member.IsIgnored());

            member.Annotations.Ignored().Reset();

            Assert.False(member.Annotations.Ignored().IsDefined());
            Assert.False(member.IsIgnored());
        }

        [Fact]
        public void Annotation_Parent()
        {
            ClassTypeMember member = new ClassTypeMember();

            member.Parent(true);

            Assert.True(member.Annotations.Parent().IsDefined());
            Assert.True(member.IsParent());

            member.Parent(false);

            Assert.True(member.Annotations.Parent().IsDefined());
            Assert.False(member.IsParent());

            member.Annotations.Parent().Reset();

            Assert.False(member.Annotations.Parent().IsDefined());
            Assert.False(member.IsParent());
        }

        [Fact]
        public void Annotation_Primitive()
        {
            ClassTypeMember member = new ClassTypeMember();

            member.Primitive(true);

            Assert.True(member.Annotations.Primitive().IsDefined());
            Assert.True(member.IsSetAsPrimitive());

            member.Primitive(false);

            Assert.True(member.Annotations.Primitive().IsDefined());
            Assert.False(member.IsSetAsPrimitive());

            member.Annotations.Primitive().Reset();

            Assert.False(member.Annotations.Primitive().IsDefined());
            Assert.False(member.IsSetAsPrimitive());
        }

        [Fact]
        public void Annotation_Discriminator()
        {
            IType type = new ClassType();

            type.SetDiscriminator("SOME_PROP", new Dictionary<object, Type>
            {
                { "a", typeof(int) },
                { "b", typeof(string) }
            });

            Assert.True(type.Annotations.DiscriminatorName().IsDefined());
            Assert.True(type.Annotations.DiscriminatorValues().IsDefined());

            var result = type.GetDiscriminator(out var setName, out var setValues);

            Assert.True(result);

            Assert.Equal("SOME_PROP", setName);
            Assert.NotNull(setValues);

            Assert.Equal(typeof(int), setValues["a"]);
            Assert.Equal(typeof(string), setValues["b"]);
        }


        [Fact]
        public void Annotation_ConcurrencyToken()
        {
            IType type = new ClassType();

            type.SetConcurrencyTokenName("SOME_PROP");

            Assert.True(type.Annotations.ConcurrencyTokenName().IsDefined());

            string token = type.GetConcurrencyTokenName();

            Assert.Equal("SOME_PROP", token);
        }
    }
}
