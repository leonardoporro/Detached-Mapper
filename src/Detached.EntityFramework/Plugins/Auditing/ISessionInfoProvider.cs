using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Plugins.Auditing
{
    /// <summary>
    /// Provides current user and date time to the auditing plugin.
    /// </summary>
    public interface ISessionInfoProvider
    {
        /// <summary>
        /// Gets the current logged user, or null if nobody is logged in.
        /// </summary>
        /// <returns>The current user name, or null.</returns>
        string GetCurrentUser();

        /// <summary>
        /// Gets the current date and time. It should probably be in universal time, for
        /// centralized databases.
        /// </summary>
        /// <returns></returns>
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
