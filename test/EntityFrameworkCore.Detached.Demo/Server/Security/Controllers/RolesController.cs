using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Demo.Security.Models;
using EntityFrameworkCore.Detached.Demo.Server.Core;

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
