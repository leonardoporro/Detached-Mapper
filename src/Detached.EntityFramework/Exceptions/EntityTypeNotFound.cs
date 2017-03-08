using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Exceptions
{
    public class EntityTypeNotFound : Exception
    {
        public Type ClrType { get; set; }

        public override string Message
        {
            get
            {
                return $"Entity type for clr type '{ClrType.FullName}' was not found.";
            }
        }
    }
}
