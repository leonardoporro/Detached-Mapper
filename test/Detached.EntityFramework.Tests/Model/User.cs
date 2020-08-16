using Detached.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests.Model
{
    [Entity]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Role> Roles { get; set; }

        [Composition]
        public List<Address> Addresses { get; set; }

        public UserType UserType { get; set; }

        [Composition]
        public UserProfile Profile { get; set; }
    }
}