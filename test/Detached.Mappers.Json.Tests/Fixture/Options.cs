using Detached.Annotations;

namespace Detached.Mappers.Json.Tests.Fixture
{
    [Entity]
    public class Options
    {
        [Composition]
        public List<ConnectionOptions> ConnectionStrings { get; set; }

        public LoggingOptions Logging { get; set; }

        public string AllowedHosts { get; set; }
    }
}
