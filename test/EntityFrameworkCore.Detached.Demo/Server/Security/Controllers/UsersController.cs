using EntityFrameworkCore.Detached.Demo.Controllers;
using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Demo.Security.Models;

namespace EntityFrameworkCore.Detached.Demo.Server.Security.Controllers
{
    public class UsersController : ControllerBase<DefaultContext, User, UserQuery>
    {
        public UsersController(IDetachedContext<DefaultContext> detachedContext) 
            : base(detachedContext)
        {
        }
    }
}
