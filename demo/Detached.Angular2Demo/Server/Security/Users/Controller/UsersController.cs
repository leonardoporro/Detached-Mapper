using Detached.Angular2Demo.Server.Security.Users.Model;
using Detached.Angular2Demo.Server.Security.Users.Services;
using Detached.Mvc.Controllers;
using Detached.Mvc.Errors;
using Detached.Mvc.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Detached.Angular2Demo.Server.Security.Users.Controllers
{
    [ApiErrors]
    [ModelErrors]
    public class UsersController : DetachedController<User, UserQuery>
    {
        public UsersController(IUserService userService) 
            : base(userService)
        {
        }
    }
}