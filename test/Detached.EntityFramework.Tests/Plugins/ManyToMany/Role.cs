using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests.Plugins.ManyToMany
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
