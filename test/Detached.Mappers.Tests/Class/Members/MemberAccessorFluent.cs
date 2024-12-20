﻿using Detached.Mappers.Options;
using Xunit;

namespace Detached.Mappers.Tests.Class.Members
{
    public class MemberAccessorFluent
    {
        [Fact]
        public void configure_member_getter_fluent()
        {
            var options = new MapperOptionsBuilder()
                .Type<User>()
                    .Member(u => u.Name)
                    .Getter((user, context) => user.Name + "+1")
                .Options;

            Mapper mapper = new Mapper(options);

            User source = new User { Id = 1, Name = "user" };
            User target = new User();

            mapper.Map(source, target);

            Assert.Equal("user+1", target.Name);
        }

        [Fact]
        public void configure_member_setter_fluent()
        {
            var options = new MapperOptionsBuilder()
                .Type<User>().Member(u => u.Name).Setter((user, value, context) =>
                    {
                        user.Name = value + "+1";
                    })
                .Options;

            Mapper mapper = new Mapper(options);

            User source = new User { Id = 1, Name = "user" };
            User target = new User();

            mapper.Map(source, target);

            Assert.Equal("user+1", target.Name);
        }


        class User
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        class UserDto
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
