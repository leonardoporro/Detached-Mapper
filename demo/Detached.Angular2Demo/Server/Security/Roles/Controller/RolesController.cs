using Detached.Angular2Demo.Server.Security.Roles.Model;
using Detached.Angular2Demo.Server.Security.Roles.Services;
using Detached.Mvc.Controllers;
using Detached.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Detached.Angular2Demo.Server.Security.Roles.Controller
{
    public class RolesController : DetachedController<Role, RoleQuery>
    {
        public RolesController(IRoleService detachedService) 
            : base(detachedService)
        {
        }
    }
}
