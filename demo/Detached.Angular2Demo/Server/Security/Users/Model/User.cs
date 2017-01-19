using Detached.Angular2Demo.Server.Security.Roles.Model;
using Detached.DataAnnotations.Plugins.ManyToMany;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Angular2Demo.Server.Security.Users.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "hola")]
        public string Name { get; set; }

        [ManyToMany(nameof(Roles)), JsonIgnore]
        public IList<UserRoles> UserRoles { get; set; }

        [NotMapped]
        public IList<Role> Roles { get; set; }
    }
}
