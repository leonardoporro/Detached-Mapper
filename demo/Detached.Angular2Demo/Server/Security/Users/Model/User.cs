using Detached.Angular2Demo.Server.Security.Roles.Model;
using Detached.DataAnnotations.Plugins.ManyToMany;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Angular2Demo.Server.Security.Users.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }
        
        public string City { get; set; }

        [ManyToMany(nameof(Roles)), JsonIgnore]
        public IList<UserRoles> UserRoles { get; set; }

        [NotMapped]
        public IList<Role> Roles { get; set; }

        public bool IsEnabled { get; set; }
    }
}