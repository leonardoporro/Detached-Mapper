using System;

namespace Detached.Mappers.Patching
{
    public class PatchProxyTypeException : Exception
    {
        public PatchProxyTypeException(string message)
            : base(message)
        {
        }
    }
}