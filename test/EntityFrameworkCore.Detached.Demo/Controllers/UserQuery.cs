using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Tools;
using System.Linq;

namespace EntityFrameworkCore.Detached.Demo.Controllers
{
    public class UserQuery : IQuery<User>
    {
        public string FullText { get; set; }

        public string OrderBy { get; set; } = nameof(User.Name);

        public string FilterBy { get; set; }

        public IQueryable<User> Apply(IQueryable<User> query)
        {
            query.Where(u => u.Name.Contains(FullText));

            return query;
        }
    }
}
