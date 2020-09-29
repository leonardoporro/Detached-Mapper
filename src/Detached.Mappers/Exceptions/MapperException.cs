using System;

namespace Detached.Mappers.Exceptions
{
    public class MapperException : Exception
    {
        public MapperException(string message) 
            : base(message)
        {
        }
    }
}