using Detached.Angular2Demo.Model;
using Detached.Angular2Demo.Server.Security.Users.Model;
using Detached.EntityFramework;
using Detached.Services;

namespace Detached.Angular2Demo.Server.Security.Users.Services
{
    public class UserService : DetachedService<DefaultContext, User, UserQuery>, IUserService
    {
        public UserService(IDetachedContext<DefaultContext> detachedContext) : base(detachedContext)
        {
        }
    }
}
