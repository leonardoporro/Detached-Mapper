using Detached.Angular2Demo.Server.Security.Invoices;
using Detached.Angular2Demo.Server.Security.Roles.Model;
using Detached.Angular2Demo.Server.Security.Users.Model;
using Microsoft.EntityFrameworkCore;

namespace Detached.Angular2Demo.Model
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
    }
}
