using System;

namespace Detached.Mapping.Exceptions
{
    public class MapperException : Exception
    {
        public MapperException(string message) 
            : base(message)
        {
        }
    }
}