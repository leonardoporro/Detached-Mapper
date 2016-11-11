using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Services
{
    public class DetachedServices : IDetachedServices
    {
        IDetachedContext _detachedContext;

        public DetachedServices()
        {
        }

        public IDetachedContext DetachedContext
        {
            get
            {
                return _detachedContext;
            }
        }

        public void Initialize(IDetachedContext detachedContext)
        {
            _detachedContext = detachedContext;
        }
    }
}
