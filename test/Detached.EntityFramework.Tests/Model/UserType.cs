using Detached.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests.Model
{
    [Entity]
    public class UserType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}