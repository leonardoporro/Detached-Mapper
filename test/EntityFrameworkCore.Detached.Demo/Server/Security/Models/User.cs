using EntityFrameworkCore.Detached.DataAnnotations.Plugins.ManyToMany;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Security.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ManyToMany(nameof(Roles)), JsonIgnore]
        public IList<UserRoles> UserRoles { get; set; }

        [NotMapped]
        public IList<Role> Roles { get; set; }
    }
}
