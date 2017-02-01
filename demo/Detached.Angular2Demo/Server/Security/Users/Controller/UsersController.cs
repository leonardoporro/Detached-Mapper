using Detached.Angular2Demo.Server.Security.Users.Model;
using Detached.Angular2Demo.Server.Security.Users.Services;
using Detached.Mvc.Controllers;
using Detached.Mvc.Validation;
using Detached.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Detached.Angular2Demo.Server.Security.Users.Controllers
{
    [ValidateModel]
    public class UsersController : DetachedController<User, UserQuery>
    {
        public UsersController(IUserService userService) 
            : base(userService)
        {
        }
    }
}
