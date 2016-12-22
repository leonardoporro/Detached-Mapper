using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Security.Models
{
    public class UserRoles
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
