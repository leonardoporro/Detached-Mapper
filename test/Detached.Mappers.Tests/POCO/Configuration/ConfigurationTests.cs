using Xunit;

namespace Detached.Mappers.Tests.POCO.Configuration
{
    public class ConfigurationTests
    {
        [Fact]
        public void map_renamed_property()
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

        [Fact]
        public void not_mapped_member()
        {
            MapperOptions opts = new MapperOptions();
            opts.Type<User>()
                .FromType<UserDTO>()
                .Member(u => u.Id).FromMember(u => u.Key)
                .Member(u => u.Name).NotMapped();

            Mapper mapper = new Mapper(opts);
            User user = mapper.Map<UserDTO, User>(new UserDTO { Key = 1, UserName = "leo" });
            Assert.Equal(1, user.Id);
            Assert.Null(user.Name);
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
