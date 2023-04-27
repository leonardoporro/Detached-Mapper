using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework
{
    public class EFMapContextModel
    {
        public Dictionary<string, string> ConcurrencyTokens { get; } = new Dictionary<string, string>();
    }
}
