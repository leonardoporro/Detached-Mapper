using Detached.Angular2Demo.Server.Security.Users.Model;
using Detached.Angular2Demo.Server.Security.Users.Services;
using EntityFrameworkCore.Detached.Demo.Server.Core;

namespace Detached.Angular2Demo.Server.Security.Users.Controllers
{
    public class UsersController : DetachedController<User, UserQuery>
    {
        public UsersController(IUserService userService) 
            : base(userService)
        {
        }
    }
}
