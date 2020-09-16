using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests.Model
{
    public class Role
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual List<User> Users { get; set; }
    }
}