using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.Auditing
{
    public interface ISessionInfoProvider
    {
        string GetCurrentUser();

        DateTime GetCurrentDateTime();
    }

    public class DelegateSessionInfoProvider : ISessionInfoProvider
    {
        Func<string> _getCurrentUser;

        public DelegateSessionInfoProvider(Func<string> getCurrentUser)
        {
            _getCurrentUser = getCurrentUser;
        }

        public string GetCurrentUser()
        {
            return _getCurrentUser();
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
