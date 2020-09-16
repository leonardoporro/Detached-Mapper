using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests.Model
{
    public class UserProfile
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
    }
}