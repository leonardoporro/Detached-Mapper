using Detached.Angular2Demo.Server.Security.Users.Model;
using Detached.Services;
using System.Linq;

namespace Detached.Angular2Demo.Server.Security.Users.Services
{
    public class UserQuery : DetachedQuery<User>
    {
        public string Name { get; set; }

        protected override IQueryable<User> ApplyCustom(IQueryable<User> query)
        {
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(n => n.Name.Contains(Name));

            if (!string.IsNullOrEmpty(SearchText))
                query = query.Where(n => n.Name.Contains(SearchText));

            return query;
        }
    }
}
