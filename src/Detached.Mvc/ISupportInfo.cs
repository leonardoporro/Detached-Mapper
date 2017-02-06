using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization
{
    public interface ISupportInfo
    {
        string Email { get; }
        string Phone { get; }
        string Address { get; }
        string Custom { get; set; }
    }
}