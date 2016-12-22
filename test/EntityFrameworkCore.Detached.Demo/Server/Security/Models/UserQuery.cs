using EntityFrameworkCore.Detached.Demo.Controllers;
using EntityFrameworkCore.Detached.Demo.Security.Models;
using System.Linq;

namespace EntityFrameworkCore.Detached.Demo.Server.Security.Controllers
{
    public class UserQuery : QueryBase<User>
    {
        public string Name { get; set; }

        protected override IQueryable<User> ApplyCustom(IQueryable<User> query)
        {
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(n => n.Name.Contains(this.Name));

            return query;
        }
    }
}
