using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests
{
    public class OwnedReference
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
