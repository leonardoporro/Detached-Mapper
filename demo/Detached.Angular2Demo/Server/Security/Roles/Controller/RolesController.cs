using Detached.Angular2Demo.Server.Security.Roles.Model;
using Detached.Angular2Demo.Server.Security.Roles.Services;
using EntityFrameworkCore.Detached.Demo.Server.Core;

namespace Detached.Angular2Demo.Server.Security.Roles.Controller
{
    public class RolesController : DetachedController<Role, RoleQuery>
    {
        public RolesController(IRoleService service)
            : base(service)
        {

        }
    }
}
