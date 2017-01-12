using Detached.EntityFramework.Plugins.Auditing;
using System;

namespace Detached.EntityFramework.Tests.Plugins.Auditing
{
    public class SessionInfoProvider: ISessionInfoProvider
    {
        public static SessionInfoProvider Default = new SessionInfoProvider();

        public string CurrentUser { get; set; } = "System";

        public DateTime DateTime { get; set; } = DateTime.Now;

        DateTime ISessionInfoProvider.GetCurrentDateTime()
        {
            return DateTime;
        }

        string ISessionInfoProvider.GetCurrentUser()
        {
            return CurrentUser;
        }
    }
}
