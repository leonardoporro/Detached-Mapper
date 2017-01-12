using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Type EntityType { get; set; }

        public object[] KeyValues { get; set; }

        public override string Message
        {
            get
            {
                return $"Entity of type '{EntityType.Name}' with key '{string.Join(",", KeyValues)}' was not found in the database. This means that the entity was deleted or never existed.";
            }
        }
    }
}
