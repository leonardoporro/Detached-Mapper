using Detached.Annotations;
using System.Collections.Generic;

namespace Detached.Mappers.Tests.Json.Fixture
{
    [Entity]
    public class AppOptions
    {
        [Composition]
        public List<ConnectionOptions> ConnectionStrings { get; set; }

        public LoggingOptions Logging { get; set; }

        public string AllowedHosts { get; set; }
    }
}
