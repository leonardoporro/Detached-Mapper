using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Configuration
{
    public class ConfigurationTests
    {
        [Fact]
        public void map_type_pair()
        {
            MapperOptions opts = new MapperOptions();
            opts.Type<User>()
                .FromType<UserDTO>()
                .Member(u => u.Id).FromMember(u => u.Key)
                .Member(u => u.Name).FromMember(u => u.UserName);

            Mapper mapper = new Mapper(opts);
            User user = mapper.Map<UserDTO, User>(new UserDTO { Key = 1, UserName = "leo" });
            Assert.Equal(1, user.Id);
            Assert.Equal("leo", user.Name);
        }

       
        class User
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        class UserDTO
        {
            public int Key { get; set; }

            public string UserName { get; set; }
        }
    }
}
