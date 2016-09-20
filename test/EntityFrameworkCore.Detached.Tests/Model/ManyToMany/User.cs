using EntityFrameworkCore.Detached.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tests.Model.ManyToMany
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ManyToMany]
        public ICollection<Role> Roles { get; set; }
    }
}
