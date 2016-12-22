using EntityFrameworkCore.Detached.Demo.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Detached.Demo.Model
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
    }
}
