using Detached.Angular2Demo.Server.Security.Users.Model;
using Detached.Services;

namespace Detached.Angular2Demo.Server.Security.Users.Services
{
    public interface IUserService : IDetachedService<User, UserQuery>
    {
    }
}
