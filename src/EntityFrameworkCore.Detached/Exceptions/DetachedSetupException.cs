using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Exceptions
{
    public class DetachedSetupException : Exception
    {
        Type _dbContextType;

        public DetachedSetupException(Type dbContextType)
        {
            _dbContextType = dbContextType;
        }

        public Type DbContextType
        {
            get
            {
                return _dbContextType;
            }
        }

        public override string Message
        {
            get
            {
                return $"Detached is not set up for context {_dbContextType.Name}. Please ensure that .UseDetached is called and, for DI, the context has a constructor accepting DbContextOptions as a parameter.";
            }
        }
    }
}
