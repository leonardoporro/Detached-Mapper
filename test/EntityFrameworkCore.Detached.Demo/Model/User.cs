using EntityFrameworkCore.Detached.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Detached.Demo.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Role> Roles { get; set; }

        public IList<Dependants> Dependants { get; set; }
    }
}
