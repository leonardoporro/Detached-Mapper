using EntityFrameworkCore.Detached.DataAnnotations;
using EntityFrameworkCore.Detached.DataAnnotations.Plugins.KeyAnnotation;
using EntityFrameworkCore.Detached.DataAnnotations.Plugins.ManyToMany;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore.Detached.Tests.Plugins.ManyToMany
{
    public class UserRoles
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
