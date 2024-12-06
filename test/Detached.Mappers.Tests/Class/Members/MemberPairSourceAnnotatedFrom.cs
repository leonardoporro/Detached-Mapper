using Detached.Annotations;
using Detached.Mappers.Options;
using System;
using Xunit;

namespace Detached.Mappers.Tests.Class.Members
{
    public class ConfigureMemberPairSourceAnnotated
    {
        [Fact]
        public void configure_member_pair_source_annotated()
        {
            MapperOptions opts = new MapperOptions();
            Mapper mapper = new Mapper(opts);

            User user = mapper.Map<UserDto, User>(new UserDto { Key = 1, UserName = "leo" });

            Assert.Equal(1, user.Id);
            Assert.Equal("leo", user.Name);
        }

        class User
        {
            [MapFrom(typeof(UserDto), "Key")]
            public int Id { get; set; }

            [MapFrom(typeof(UserDto), "UserName")]
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