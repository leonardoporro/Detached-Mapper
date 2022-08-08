using Detached.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Json.Tests.Fixture
{
    [Entity]
    public class ConnectionOptions
    {
        [Key]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
