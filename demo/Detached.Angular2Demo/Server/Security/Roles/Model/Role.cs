using System.ComponentModel.DataAnnotations;

namespace Detached.Angular2Demo.Server.Security.Roles.Model
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
