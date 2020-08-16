using Detached.Patch;
using System;
using Xunit;

namespace Detached.Tests.Patch
{
    public class PatchTests
    {
        readonly PatchProxyTypeFactory _proxyFactory = new PatchProxyTypeFactory();

        [Fact]
        public void create_proxy_patch()
        {
            Type proxyType = _proxyFactory.Create<Entity>();
            Entity entity = (Entity)Activator.CreateInstance(proxyType);

            entity.Name = "newName";

            IPatch entityChanges = (IPatch)entity;

            Assert.True(entityChanges.IsSet("Name"));
            Assert.False(entityChanges.IsSet("Date"));

            entityChanges.Reset();

            Assert.False(entityChanges.IsSet("Name"));
            Assert.False(entityChanges.IsSet("Date"));
        }

        public class Entity
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime Date { get; set; }
        }

        [Fact]
        public void fail_create_when_no_constructor()
        {
            Assert.Throws<PatchProxyTypeException>(() =>_proxyFactory.Create<EntityWithConstructor>());
        }
         
        public class EntityWithConstructor
        {
            public EntityWithConstructor(int id)
            {

            }

            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime Date { get; set; }
        }
    }
}
