using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests
{
    public class AssociatedReference
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
