using System;

namespace Detached.Patch
{
    public class PatchProxyTypeException : Exception
    {
        public PatchProxyTypeException(string message)
            : base(message)
        {
        }
    }
}