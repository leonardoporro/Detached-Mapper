using Detached.Angular2Demo.Model;
using Detached.Angular2Demo.Server.Security.Roles.Model;
using Detached.EntityFramework;
using Detached.Services;

namespace Detached.Angular2Demo.Server.Security.Roles.Services
{
    public class RoleService : DetachedService<DefaultContext, Role, RoleQuery>, IRoleService
    {
        public RoleService(IDetachedContext<DefaultContext> detachedContext)
            : base(detachedContext)
        {
        }
    }
}
