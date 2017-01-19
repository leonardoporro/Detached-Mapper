using Detached.Angular2Demo.Server.Security.Roles.Model;
using Detached.Services;

namespace Detached.Angular2Demo.Server.Security.Roles.Services
{
    public interface IRoleService : IDetachedService<Role, RoleQuery>
    {
    }
}
