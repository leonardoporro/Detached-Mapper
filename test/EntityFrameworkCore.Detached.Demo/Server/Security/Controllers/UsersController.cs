using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Demo.Security.Models;
using EntityFrameworkCore.Detached.Demo.Server.Core;

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
