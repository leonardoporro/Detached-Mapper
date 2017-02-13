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
    public interface ISupportInfo
    {
        /// <summary>
        /// Company's support E-mail address.
        /// </summary>
        string Email { get; }
        /// <summary>
        /// Company's support Phone address.
        /// </summary>
        string Phone { get; }
        /// <summary>
        /// Company's support office location.
        /// </summary>
        string Address { get; }
        /// <summary>
        /// Custom info for the validation/error message.
        /// </summary>
        string Custom { get; set; }
    }
}