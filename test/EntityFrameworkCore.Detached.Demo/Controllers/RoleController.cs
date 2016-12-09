using EntityFrameworkCore.Detached.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Controllers
{
    public class RoleController : ControllerBase<DemoContext, Role, RoleQuery>
    {
        public RoleController(IDetachedContext<DemoContext> detachedContext)
            : base(detachedContext)
        {

        }
    }
}
