using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Tools;
using System.Linq;
using System;

namespace EntityFrameworkCore.Detached.Demo.Controllers
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
