using Detached.Annotations;
using System;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Configuration
{
    public class MemberPairSourceAnnotatedTo
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
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime ModifiedDate { get; set; }
        }

        class UserDto
        {
            [MapTo(typeof(User), "Id")]
            public int Key { get; set; }

            [MapTo(typeof(User), "Name")]
            public string UserName { get; set; }
        }
    }
}