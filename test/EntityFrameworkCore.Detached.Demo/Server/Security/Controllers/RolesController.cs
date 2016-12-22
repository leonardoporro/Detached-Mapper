using EntityFrameworkCore.Detached.Demo.Controllers;
using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Demo.Security.Models;

namespace EntityFrameworkCore.Detached.Demo.Server.Security.Controllers
{
    public class RolesController : ControllerBase<DefaultContext, Role, RoleQuery>
    {
        public RolesController(IDetachedContext<DefaultContext> detachedContext)
            : base(detachedContext)
        {

        }
    }
}
