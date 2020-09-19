using System;

namespace Detached.Patching
{
    public class PatchProxyTypeException : Exception
    {
        public PatchProxyTypeException(string message)
            : base(message)
        {
        }
    }
}