using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Services
{
    public interface IDetachedServices
    {
        IDetachedContext DetachedContext { get; }

        void Initialize(IDetachedContext detachedContext);
    }
}
