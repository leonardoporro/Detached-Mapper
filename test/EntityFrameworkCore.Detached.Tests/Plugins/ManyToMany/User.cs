using EntityFrameworkCore.Detached.DataAnnotations.Plugins.ManyToMany;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Detached.Tests.Plugins.ManyToMany
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<UserRoles> UserRoles { get; set; }

        [ManyToMany(nameof(UserRoles))]
        public IList<Role> Roles { get; set; }
    }
}
