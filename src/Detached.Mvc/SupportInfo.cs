using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization
{
    /// <summary>
    /// Provides some info to be added to validation and error messages.
    /// Used internally by ErrorLocalizer.
    /// </summary>
    public class SupportInfo : ISupportInfo
    {
        /// <summary>
        /// Company's support E-mail address.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Company's support Phone address.
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Company's support office location.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Custom info for the validation/error message.
        /// </summary>
        public string Custom { get; set; }
    }
}
