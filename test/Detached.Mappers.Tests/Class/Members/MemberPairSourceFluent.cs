using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using System;
using Xunit;

namespace Detached.Mappers.Tests.Class.Members
{
    public class MemberPairSourceFluent
    {
        [Fact]
        public void configure_member_pair_source()
        {
            var options = new MapperOptionsBuilder()
                .Type<User>()
                    .FromType<UserDto>()
                        .Member(u => u.Id).FromMember(u => u.Key)
                        .Member(u => u.Name).FromMember(u => u.UserName)
                .Options;

            Mapper mapper = new Mapper(options);
            User user = mapper.Map<UserDto, User>(new UserDto { Key = 1, UserName = "leo" });
            Assert.Equal(1, user.Id);
            Assert.Equal("leo", user.Name);
        }

        [Fact]
        public void configure_member_pair_exclude_source()
        {
            var options = new MapperOptionsBuilder()
                .Type<User>()
                    .FromType<UserDto>()
                        .Member(u => u.Id).FromMember(u => u.Key)
                        .Member(u => u.Name).Exclude()
                .Options;

            Mapper mapper = new Mapper(options);
            User user = mapper.Map<UserDto, User>(new UserDto { Key = 1, UserName = "leo" });
            Assert.Equal(1, user.Id);
            Assert.Null(user.Name);
        }

        [Fact]
        public void configure_member_pair_custom_source()
        {
            DateTime dateTime = DateTime.Now;

            var options = new MapperOptionsBuilder()
                .Type<User>()
                    .FromType<UserDto>()
                        .Member(u => u.Id).FromMember(u => u.Key)
                        .Member(u => u.Name).FromMember(u => u.UserName)
                        .Member(u => u.ModifiedDate).FromValue((u, c) => dateTime)
                .Options;

            Mapper mapper = new Mapper(options);
            User user = mapper.Map<UserDto, User>(new UserDto { Key = 1, UserName = "leo" });
            Assert.Equal(1, user.Id);
            Assert.Equal("leo", user.Name);
            Assert.Equal(dateTime, user.ModifiedDate);
        }

        class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime ModifiedDate { get; set; }
        }

        class UserDto
        {
            public int Key { get; set; }

            public string UserName { get; set; }
        }
    }
}
