using EntityFrameworkCore.Detached.DataAnnotations.Plugins.ManyToMany;

namespace EntityFrameworkCore.Detached.Tests.Plugins.ManyToMany
{
    public class UserRoles
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
