using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization
{
    public class SupportInfo : ISupportInfo
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Custom { get; set; }
    }
}
