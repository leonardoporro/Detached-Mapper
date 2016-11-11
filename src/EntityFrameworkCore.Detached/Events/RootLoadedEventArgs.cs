using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Events
{
    public class RootLoadedEventArgs
    {
        public object Root { get; set; }
    }
}
